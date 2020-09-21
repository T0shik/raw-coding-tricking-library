using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
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
    [ApiController]
    [Route("api/tricks")]
    public class TricksController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public TricksController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IEnumerable<object> All() => _ctx.Tricks
            .Where(x => x.Active)
            .Select(TrickViewModels.Projection).ToList();

        [HttpGet("{id}")]
        public object Get(string id) =>
            _ctx.Tricks
                .Where(x => x.Active)
                .Where(x => x.Slug.Equals(id, StringComparison.InvariantCultureIgnoreCase))
                .Select(TrickViewModels.Projection)
                .FirstOrDefault();

        [HttpGet("{trickId}/submissions")]
        public IEnumerable<Submission> ListSubmissionsForTrick(string trickId) =>
            _ctx.Submissions
                .Include(x => x.Video)
                .Where(x => x.TrickId.Equals(trickId, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

        [HttpPost]
        public async Task<object> Create([FromBody] TrickForm trickForm)
        {
            var trick = new Trick
            {
                Slug = trickForm.Name.Replace(" ", "-").ToLowerInvariant(),
                Name = trickForm.Name,
                Version = 1,
                Description = trickForm.Description,
                Difficulty = trickForm.Difficulty,
                TrickCategories = trickForm.Categories.Select(x => new TrickCategory {CategoryId = x}).ToList()
            };
            _ctx.Add(trick);
            _ctx.Add(new ModerationItem
            {
                Target = trick.Slug,
                TargetVersion = trick.Version,
                Type = ModerationTypes.Trick,
            });
            await _ctx.SaveChangesAsync();
            return TrickViewModels.Create(trick);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TrickForm trickForm)
        {
            var trick = _ctx.Tricks.FirstOrDefault(x => x.Slug == trickForm.Id);
            if (trick == null)
            {
                return NoContent();
            }

            var newTrick = new Trick
            {
                Slug = trick.Slug,
                Name = trick.Name,
                Version = _ctx.Tricks.LatestVersion(1),
                Description = trickForm.Description,
                Difficulty = trickForm.Difficulty,
                Prerequisites = trickForm.Prerequisites
                    .Select(x => new TrickRelationship {PrerequisiteId = x})
                    .ToList(),
                Progressions = trickForm.Progressions
                    .Select(x => new TrickRelationship {ProgressionId = x})
                    .ToList(),
                TrickCategories = trickForm.Categories.Select(x => new TrickCategory {CategoryId = x}).ToList()
            };

            _ctx.Add(newTrick);
            _ctx.Add(new ModerationItem
            {
                Target = trick.Slug,
                TargetVersion = newTrick.Version,
                Type = ModerationTypes.Trick,
            });
            await _ctx.SaveChangesAsync();

            // todo redirect to the mod item instead of returning the trick
            return Ok(TrickViewModels.Create(newTrick));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var trick = _ctx.Tricks.FirstOrDefault(x => x.Slug.Equals(id));
            trick.Deleted = true;
            await _ctx.SaveChangesAsync();
            return Ok();
        }
    }
}