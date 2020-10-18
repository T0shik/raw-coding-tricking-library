using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrickingLibrary.Api.Form;
using TrickingLibrary.Api.ViewModels;
using TrickingLibrary.Data;
using TrickingLibrary.Models;
using TrickingLibrary.Models.Abstractions;

namespace TrickingLibrary.Api.Controllers
{
    [Route("api/comments")]
    [Authorize(TrickingLibraryConstants.Policies.User)]
    public class CommentController : ApiController
    {
        private readonly AppDbContext _ctx;

        public CommentController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("{parentId}/{parentType}")]
        public IEnumerable<object> GetReplies(
            int parentId,
            CommentCreationContext.ParentType parentType,
            [FromQuery] FeedQuery feedQuery
        )
        {
            Expression<Func<Comment, bool>> filter = parentType switch
            {
                CommentCreationContext.ParentType.ModerationItem => comment => comment.ModerationItemId == parentId,
                CommentCreationContext.ParentType.Submission => comment => comment.SubmissionId == parentId,
                CommentCreationContext.ParentType.Comment => comment => comment.ParentId == parentId,
                _ => throw new ArgumentException(),
            };

            return _ctx.Comments
                .Where(filter)
                .OrderFeed(feedQuery)
                .Select(CommentViewModel.Projection)
                .ToList();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CommentForm commentForm,
            [FromServices] CommentCreationContext commentCreationContext)
        {
            try
            {
                var comment = await commentCreationContext
                    .Setup(UserId)
                    .CreateAsync(commentForm);

                return Ok(CommentViewModel.Create(comment));
            }
            catch (CommentCreationContext.ParentNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}