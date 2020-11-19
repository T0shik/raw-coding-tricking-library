using System;
using System.Linq;
using System.Linq.Expressions;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.ViewModels
{
    public static class UserViewModels
    {
        public static Expression<Func<User, object>> Projection =>
            user => new
            {
                user.Id,
                user.Username,
                user.Image,
                Submissions = user.Submissions.AsQueryable().Select(x => new
                    {
                        x.Id,
                        x.TrickId,
                        Score = x.Votes.Sum(v => v.Value),
                    })
                    .ToList(),
            };

        public static readonly Func<User, object> CreateFlatCache = FlatProjection.Compile();
        public static object CreateFlat(User user) => CreateFlatCache(user);

        public static Expression<Func<User, object>> FlatProjection =>
            user => new
            {
                user.Id,
                user.Username,
                user.Image,
            };

        public static Expression<Func<User, object>> ProfileProjection(string role) =>
            user => new
            {
                user.Id,
                user.Username,
                user.Image,
                Submissions = user.Submissions.AsQueryable().Select(x => new
                    {
                        x.Id,
                        x.TrickId,
                        Score = x.Votes.Sum(v => v.Value),
                    })
                    .ToList(),
                Role = role,
            };
    }
}