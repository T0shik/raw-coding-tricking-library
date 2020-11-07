using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
    [Route("api/moderation-items")]
    public class ModerationItemController : ApiController
    {
        private readonly AppDbContext _ctx;

        public ModerationItemController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public object All([FromQuery] FeedQuery feedQuery)
        {
            var moderationItems = _ctx.ModerationItems
                .Include(x => x.User)
                .Include(x => x.Reviews)
                .Where(x => !x.Deleted)
                .OrderFeed(feedQuery)
                .ToList();

            var targetMapping = new Dictionary<int, object>();
            foreach (var group in moderationItems.GroupBy(x => x.Type))
            {
                var targetIds = group.Select(m => m.Target).ToArray();
                if (group.Key == ModerationTypes.Trick)
                {
                    _ctx.Tricks
                        .Where(t => targetIds.Contains(t.Id))
                        .ToList()
                        .ForEach(trick => targetMapping[trick.Id] = TrickViewModels.CreateFlat(trick));
                }
            }

            return moderationItems.Select(x => new
            {
                x.Id,
                x.Current,
                x.Target,
                x.Reason,
                x.Type,
                Updated = x.Updated.ToLocalTime().ToString("HH:mm dd/MM/yyyy"),
                Reviews = x.Reviews.Select(y => y.Status).ToList(),
                User = UserViewModels.CreateFlat(x.User),
                TargetObject = targetMapping[x.Target],
            });
        }

        [HttpGet("{id}")]
        public object Get(int id) => _ctx.ModerationItems
            .Where(x => x.Id.Equals(id))
            .Select(ModerationItemViewModels.Projection)
            .FirstOrDefault();

        [HttpGet("{id}/reviews")]
        public IEnumerable<object> GetReviews(int id) =>
            _ctx.Reviews
                .Include(x => x.User)
                .Where(x => x.ModerationItemId.Equals(id))
                .Select(ReviewViewModels.WithUserProjection)
                .ToList();

        [HttpPut("{id}/reviews")]
        [Authorize(TrickingLibraryConstants.Policies.Mod)]
        public async Task<IActionResult> Review(int id,
            [FromBody] ReviewForm reviewForm,
            [FromServices] VersionMigrationContext migrationContext)
        {
            var modItem = _ctx.ModerationItems
                .Include(x => x.Reviews)
                .FirstOrDefault(x => x.Id == id);

            if (modItem == null)
            {
                return NoContent();
            }

            if (modItem.Deleted)
            {
                return BadRequest("Moderation item no longer exists.");
            }

            // todo make this async safe
            var review = _ctx.Reviews.FirstOrDefault(x => x.ModerationItemId == id && x.UserId == UserId);

            if (review == null)
            {
                review = new Review
                {
                    ModerationItemId = id,
                    Comment = reviewForm.Comment,
                    Status = reviewForm.Status,
                    UserId = UserId,
                };

                _ctx.Add(review);
            }
            else
            {
                review.Comment = reviewForm.Comment;
                review.Status = reviewForm.Status;
            }

            // todo use configuration replace the magic '3'
            try
            {
                int goal = 3, score = 0, wait = 0;
                foreach (var r in modItem.Reviews)
                {
                    if (r.Status == ReviewStatus.Approved)
                        score++;
                    else if (r.Status == ReviewStatus.Rejected)
                        score--;
                    else if (r.Status == ReviewStatus.Waiting)
                        wait++;
                }

                if (score >= goal + wait)
                {
                    migrationContext.Migrate(modItem);
                    modItem.Deleted = true;
                }
                else if (score <= -goal - wait)
                {
                    // todo cleanup target
                    modItem.Deleted = true;
                    modItem.Rejected = true;
                }

                modItem.Updated = DateTime.UtcNow;
                await _ctx.SaveChangesAsync();
            }
            catch (VersionMigrationContext.InvalidVersionException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}