using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Api.Form;
using TrickingLibrary.Api.ViewModels;
using TrickingLibrary.Data;
using TrickingLibrary.Models;
using TrickingLibrary.Models.Moderation;

namespace TrickingLibrary.Api.Controllers
{
    [Route("api/difficulties")]
    public class DifficultyController : ApiController
    {
        private readonly AppDbContext _ctx;

        public DifficultyController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IEnumerable<object> All() =>
            _ctx.Difficulties
                .Where(x => !x.Deleted && x.State == VersionState.Live)
                .Select(DifficultyViewModels.Projection)
                .ToList();

        [HttpGet("{value}")]
        public object Get(string value) =>
            _ctx.Difficulties
                .WhereIdOrSlug(value)
                .Select(DifficultyViewModels.Projection)
                .FirstOrDefault();

        [HttpGet("{slug}/history")]
        public IEnumerable<object> GetHistory(string slug)
        {
            return _ctx.Difficulties
                .Where(x => x.Slug.Equals(slug, StringComparison.InvariantCultureIgnoreCase)
                            && x.State != VersionState.Staged)
                .Select(DifficultyViewModels.Projection)
                .ToList();
        }

        [HttpPost]
        [Authorize(TrickingLibraryConstants.Policies.Mod)]
        public async Task<IActionResult> Create([FromBody] CreateDifficultyForm form)
        {
            var difficulty = new Difficulty
            {
                Slug = form.Name.Replace(" ", "-").ToLowerInvariant(),
                Name = form.Name,
                Description = form.Description,
                UserId = UserId,
            };
            _ctx.Add(difficulty);
            await _ctx.SaveChangesAsync();
            _ctx.Add(new ModerationItem
            {
                Target = difficulty.Id,
                UserId = UserId,
                Type = ModerationTypes.Difficulty
            });
            await _ctx.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Authorize(TrickingLibraryConstants.Policies.Mod)]
        public async Task<IActionResult> Update([FromBody] UpdateDifficultyForm form)
        {
            var difficulty = _ctx.Difficulties.FirstOrDefault(x => x.Id == form.Id);

            if (difficulty == null)
            {
                return NoContent();
            }

            var newDifficulty = new Difficulty
            {
                Slug = form.Name.Replace(" ", "-").ToLowerInvariant(),
                Name = form.Name,
                Description = form.Description,
                UserId = UserId,
            };

            newDifficulty.Version = difficulty.Slug.Equals(newDifficulty.Slug, StringComparison.InvariantCultureIgnoreCase)
                ? difficulty.Version + 1
                : 1;

            _ctx.Add(newDifficulty);

            await _ctx.SaveChangesAsync();
            _ctx.Add(new ModerationItem
            {
                Current = difficulty.Id,
                Target = newDifficulty.Id,
                UserId = UserId,
                Type = ModerationTypes.Difficulty,
            });
            await _ctx.SaveChangesAsync();
            return Ok();
        }


        [HttpPut("{current}/{target}")]
        [Authorize(TrickingLibraryConstants.Policies.Mod)]
        public async Task<IActionResult> Migrate(int current, int target)
        {
            var difficultyCount = _ctx.Difficulties.Count(x => !x.Deleted
                                                               && x.State == VersionState.Live
                                                               && (x.Id == current || x.Id == target));
            if (difficultyCount != 2)
            {
                return NoContent();
            }

            _ctx.ModerationItems.Add(new ModerationItem
            {
                Current = current,
                Target = target,
                UserId = UserId,
                Type = ModerationTypes.Difficulty,
            });
            await _ctx.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("staged")]
        [Authorize]
        public async Task<IActionResult> UpdateStaged([FromBody] UpdateDifficultyForm form)
        {
            var difficulty = _ctx.Difficulties
                .FirstOrDefault(x => x.Id == form.Id);

            if (difficulty == null) return NoContent();
            if (difficulty.UserId != UserId) return BadRequest("Can't edit this difficulty.");

            difficulty.Description = form.Description;

            await _ctx.SaveChangesAsync();

            return Ok();
        }
    }
}