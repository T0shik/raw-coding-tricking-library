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
                Score = submissions.Votes.AsQueryable().Sum(x => x.Value),
                Vote = 0,
                User = new
                {
                    submissions.User.Image,
                    submissions.User.Username,
                },
            };

        public static Expression<Func<Submission, object>> PerspectiveProjection(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return Projection;
            }

            return submissions => new
            {
                submissions.Id,
                submissions.Description,
                Video = submissions.Video.VideoLink,
                Thumb = submissions.Video.ThumbLink,
                Created = submissions.Created
                    .ToLocalTime()
                    .ToString("HH:mm dd/MM/yyyy"),
                Score = submissions.Votes.AsQueryable().Sum(x => x.Value),
                Vote = submissions.Votes.AsQueryable()
                    .Where(x => x.UserId == userId)
                    .Select(x => x.Value)
                    .FirstOrDefault(),
                User = new
                {
                    submissions.User.Image,
                    submissions.User.Username,
                },
            };
        }
    }
}