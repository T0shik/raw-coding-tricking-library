using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrickingLibrary.Api.Form;
using TrickingLibrary.Api.ViewModels;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

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

        [HttpGet("{id}/replies")]
        public IEnumerable<object> GetReplies(int id) =>
            _ctx.Comments
                .Where(x => x.ParentId == id)
                .Select(CommentViewModel.Projection)
                .ToList();

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CommentForm commentForm,
            [FromServices] CommentCreationContext commentCreationContext)
        {
            var comment = await commentCreationContext
                .Setup(UserId)
                .CreateAsync(commentForm);

            return Ok(CommentViewModel.Create(comment));
        }
    }
}