using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
    [Route("api/moderation-items")]
    public class ModerationItemController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public ModerationItemController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IEnumerable<ModerationItem> All() => _ctx.ModerationItems
            .Where(x => !x.Deleted)
            .ToList();

        [HttpGet("{id}")]
        public object Get(int id) => _ctx.ModerationItems
            .Include(x => x.Comments)
            .Include(x => x.Reviews)
            .Where(x => x.Id.Equals(id))
            .Select(ModerationItemViewModels.Projection)
            .FirstOrDefault();

        [HttpGet("{id}/reviews")]
        public IEnumerable<Review> GetReviews(int id) =>
            _ctx.Reviews
                .Where(x => x.ModerationItemId.Equals(id))
                .ToList();

        [HttpPost("{id}/reviews")]
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
            var review = new Review
            {
                ModerationItemId = id,
                Comment = reviewForm.Comment,
                Status = reviewForm.Status,
            };

            _ctx.Add(review);

            // todo use configuration replace the magic '3'
            try
            {
                if (modItem.Reviews.Count >= 3)
                {
                    migrationContext.Migrate(modItem);
                    modItem.Deleted = true;
                }

                await _ctx.SaveChangesAsync();
            }
            catch (VersionMigrationContext.InvalidVersionException e)
            {
                return BadRequest(e.Message);
            }

            return Ok(ReviewViewModel.Create(review));
        }
    }
}