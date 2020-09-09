using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public IEnumerable<ModerationItem> All() => _ctx.ModerationItems.ToList();

        [HttpGet("{id}")]
        public ModerationItem Get(int id) => _ctx.ModerationItems.FirstOrDefault(x => x.Id.Equals(id));

        [HttpGet("{id}/comments")]
        public IEnumerable<object> GetComments(int id) =>
            _ctx.Comments
                .Where(x => x.ModerationItemId.Equals(id))
                .Select(CommentViewModel.Projection)
                .ToList();

        [HttpPost("{id}/comments")]
        public async Task<IActionResult> Comment(int id, [FromBody] Comment comment)
        {
            if (!_ctx.ModerationItems.Any(x => x.Id == id))
            {
                return NoContent();
            }

            var regex = new Regex(@"\B(?<tag>@[a-zA-Z0-9-_]+)");

            comment.HtmlContent = regex.Matches(comment.Content)
                                       .Aggregate(comment.Content,
                                                  (content, match) =>
                                                  {
                                                      var tag = match.Groups["tag"].Value;
                                                      return content
                                                         .Replace(tag, $"<a href=\"{tag}-user-link\">{tag}</a>");
                                                  });

            comment.ModerationItemId = id;
            _ctx.Add(comment);
            await _ctx.SaveChangesAsync();

            return Ok(CommentViewModel.Create(comment));
        }

        [HttpGet("{id}/reviews")]
        public IEnumerable<Review> GetReviews(int id) =>
            _ctx.Reviews
                .Where(x => x.ModerationItemId.Equals(id))
                .ToList();

        [HttpPost("{id}/reviews")]
        public async Task<IActionResult> Review(int id, [FromBody] Review review)
        {
            if (!_ctx.ModerationItems.Any(x => x.Id == id))
            {
                return NoContent();
            }

            review.ModerationItemId = id;
            _ctx.Add(review);
            await _ctx.SaveChangesAsync();

            return Ok(review);
        }
    }
}