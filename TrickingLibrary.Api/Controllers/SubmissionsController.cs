using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Api.BackgroundServices.SubmissionVoting;
using TrickingLibrary.Api.BackgroundServices.VideoEditing;
using TrickingLibrary.Api.Form;
using TrickingLibrary.Api.Services.Storage;
using TrickingLibrary.Api.ViewModels;
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
        public object Get(int id) =>
            _ctx.Submissions
                .Where(x => x.Id.Equals(id))
                .Include(x => x.Video)
                .Include(x => x.User)
                .Select(SubmissionViewModels.PerspectiveProjection(UserId))
                .FirstOrDefault();


        [HttpGet("best-submission")]
        public object ListSubmissionsForTrick(string byTricks)
        {
            var trickIds = byTricks.Split(';');
            return _ctx.Submissions
                .Where(x => trickIds.Contains(x.TrickId))
                .Include(x => x.Video)
                .Include(x => x.User)
                .OrderByDescending(x => x.Votes.Sum(v => v.Value))
                .Select(SubmissionViewModels.PerspectiveProjection(UserId))
                .FirstOrDefault();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(
            [FromBody] SubmissionForm submissionForm,
            [FromServices] Channel<EditVideoMessage> channel,
            [FromServices] TemporaryFileStorage temporaryFileStorage)
        {
            if (!temporaryFileStorage.TemporaryFileExists(submissionForm.Video))
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

        [HttpPut("{id}/vote")]
        [Authorize]
        public async Task<IActionResult> Vote(
            int id,
            int value,
            [FromServices] ISubmissionVoteSink voteSink
        )
        {
            if (value != -1 && value != 1)
            {
                return BadRequest();
            }

            await voteSink.Submit(new VoteForm
            {
                SubmissionId = id,
                UserId = UserId,
                Value = value,
            });

            return Ok();
        }
    }
}