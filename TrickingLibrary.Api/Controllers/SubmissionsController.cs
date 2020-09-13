using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrickingLibrary.Api.BackgroundServices.VideoEditing;
using TrickingLibrary.Api.Form;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.Controllers
{
    [Route("api/submissions")]
    public class SubmissionsController : ApiController
    {
        private readonly AppDbContext _ctx;

        public SubmissionsController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IEnumerable<Submission> All() =>
            _ctx.Submissions
                .Where(x => x.VideoProcessed)
                .ToList();

        [HttpGet("{id}")]
        public Submission Get(int id) => _ctx.Submissions.FirstOrDefault(x => x.Id.Equals(id));

        [HttpPost]
        [Authorize(TrickingLibraryConstants.Policies.User)]
        public async Task<IActionResult> Create(
            [FromBody] SubmissionForm submissionForm,
            [FromServices] Channel<EditVideoMessage> channel,
            [FromServices] IFileManager fileManagerLocal)
        {
            if (!fileManagerLocal.TemporaryFileExists(submissionForm.Video))
            {
                return BadRequest();
            }

            var submission = new Submission
            {
                TrickId = submissionForm.TrickId,
                Description = submissionForm.Description,
                VideoProcessed = false,
                UserId = UserId
            };

            _ctx.Add(submission);
            await _ctx.SaveChangesAsync();
            await channel.Writer.WriteAsync(new EditVideoMessage
            {
                SubmissionId = submission.Id,
                Input = submissionForm.Video,
            });
            return Ok(submission);
        }

        [HttpPut]
        public async Task<Submission> Update([FromBody] Submission submission)
        {
            if (submission.Id == 0)
            {
                return null;
            }

            _ctx.Add(submission);
            await _ctx.SaveChangesAsync();
            return submission;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var submission = _ctx.Submissions.FirstOrDefault(x => x.Id.Equals(id));
            if (submission == null)
            {
                return NotFound();
            }

            submission.Deleted = true;
            await _ctx.SaveChangesAsync();
            return Ok();
        }
    }
}