using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Api.Form;
using TrickingLibrary.Api.ViewModels;
using TrickingLibrary.Data;
using TrickingLibrary.Models;
using TrickingLibrary.Models.Moderation;

namespace TrickingLibrary.Api.Controllers
{
    [Route("api/tricks")]
    public class TricksController : ApiController
    {
        private readonly AppDbContext _ctx;

        public TricksController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IEnumerable<object> All() => _ctx.Tricks
            .AsNoTracking()
            .Where(x => !x.Deleted && x.State == VersionState.Live)
            .Include(x => x.TrickCategories)
            .Include(x => x.Progressions)
            .Include(x => x.Prerequisites)
            .Include(x => x.User)
            .Select(TrickViewModels.Projection).ToList();

        [HttpGet("{value}")]
        public IActionResult Get(string value)
        {
            var trick = _ctx.Tricks
                .WhereIdOrSlug(value)
                .Include(x => x.TrickCategories)
                .Include(x => x.Progressions)
                .Include(x => x.Prerequisites)
                .Include(x => x.User)
                .Select(TrickViewModels.FullProjection)
                .FirstOrDefault();

            if (trick == null)
            {
                return NoContent();
            }

            return Ok(trick);
        }

        [HttpGet("{slug}/history")]
        public IEnumerable<object> GetHistory(string slug)
        {
            return _ctx.Tricks
                .Where(x => x.Slug.ToLower() == slug.ToLower()
                            && x.State != VersionState.Staged)
                .Include(x => x.TrickCategories)
                .Include(x => x.Progressions)
                .Include(x => x.Prerequisites)
                .Include(x => x.User)
                .Select(TrickViewModels.FullProjection)
                .ToList();
        }

        [HttpGet("{trickId}/submissions")]
        public IEnumerable<object> ListSubmissionsForTrick(string trickId, [FromQuery] FeedQuery feedQuery)
        {
            return _ctx.Submissions
                .Include(x => x.Video)
                .Include(x => x.User)
                .Where(x => x.TrickId.ToLower() == trickId.ToLower())
                .OrderFeed(feedQuery)
                .Select(SubmissionViewModels.PerspectiveProjection(UserId))
                .ToList();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateTrickForm createTrickForm)
        {
            var trick = new Trick
            {
                Slug = createTrickForm.Name.Replace(" ", "-").ToLowerInvariant(),
                Name = createTrickForm.Name,
                Version = 1,
                Description = createTrickForm.Description,
                TrickDifficulties = new List<TrickDifficulty>
                {
                    new TrickDifficulty {DifficultyId = createTrickForm.Difficulty}
                },
                Prerequisites = createTrickForm.Prerequisites
                    .Select(x => new TrickRelationship {PrerequisiteId = x})
                    .ToList(),
                Progressions = createTrickForm.Progressions
                    .Select(x => new TrickRelationship {ProgressionId = x})
                    .ToList(),
                TrickCategories = createTrickForm.Categories
                    .Select(x => new TrickCategory {CategoryId = x})
                    .ToList(),
                UserId = UserId,
            };
            _ctx.Add(trick);
            await _ctx.SaveChangesAsync();
            _ctx.Add(new ModerationItem
            {
                Target = trick.Id,
                Type = ModerationTypes.Trick,
                UserId = UserId,
            });
            await _ctx.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateTrickForm createTrickForm)
        {
            var trick = _ctx.Tricks.FirstOrDefault(x => x.Id == createTrickForm.Id);
            if (trick == null)
            {
                return NoContent();
            }

            var newTrick = new Trick
            {
                Slug = trick.Slug,
                Name = trick.Name,
                Version = trick.Version + 1,
                Description = createTrickForm.Description,
                TrickDifficulties = new List<TrickDifficulty>
                {
                    new TrickDifficulty {DifficultyId = createTrickForm.Difficulty}
                },
                Prerequisites = createTrickForm.Prerequisites
                    .Select(x => new TrickRelationship {PrerequisiteId = x})
                    .ToList(),
                Progressions = createTrickForm.Progressions
                    .Select(x => new TrickRelationship {ProgressionId = x})
                    .ToList(),
                TrickCategories = createTrickForm.Categories
                    .Select(x => new TrickCategory {CategoryId = x})
                    .ToList(),
                UserId = UserId,
            };

            _ctx.Add(newTrick);
            await _ctx.SaveChangesAsync();
            _ctx.Add(new ModerationItem
            {
                Current = trick.Id,
                Target = newTrick.Id,
                Type = ModerationTypes.Trick,
                Reason = createTrickForm.Reason,
                UserId = UserId,
            });
            await _ctx.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("staged")]
        [Authorize]
        public async Task<IActionResult> UpdateStaged([FromBody] UpdateStagedTrickForm form)
        {
            var trick = _ctx.Tricks
                .Include(x => x.TrickDifficulties)
                .Include(x => x.TrickCategories)
                .Include(x => x.Prerequisites)
                .Include(x => x.Progressions)
                .FirstOrDefault(x => x.Id == form.Id);

            if (trick == null) return NoContent();
            if (trick.UserId != UserId) return BadRequest("Can't edit this trick.");

            trick.Description = form.Description;
            trick.TrickDifficulties = new List<TrickDifficulty> {new TrickDifficulty {DifficultyId = form.Difficulty}};
            trick.Prerequisites = form.Prerequisites
                .Select(x => new TrickRelationship {PrerequisiteId = x})
                .ToList();
            trick.Progressions = form.Progressions
                .Select(x => new TrickRelationship {ProgressionId = x})
                .ToList();
            trick.TrickCategories = form.Categories
                .Select(x => new TrickCategory {CategoryId = x})
                .ToList();

            await _ctx.SaveChangesAsync();

            return Ok();
        }
    }
}