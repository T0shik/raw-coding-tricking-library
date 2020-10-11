using System;
using System.Linq;
using System.Linq.Expressions;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.ViewModels
{
    public static class SubmissionViewModels
    {
        public static readonly Func<Submission, object> Created = Projection.Compile();

        public static Expression<Func<Submission, object>> Projection =>
            submissions => new
            {
                submissions.Id,
                submissions.Description,
                Video = submissions.Video.VideoLink,
                Thumb = submissions.Video.ThumbLink,
                Created = submissions.Created
                    .ToLocalTime()
                    .ToString("HH:mm dd/MM/yyyy"),
                UpVotes = submissions.UpVotes.Count,
                User = new
                {
                    submissions.User.Image,
                    submissions.User.Username,
                },
            };
    }
}