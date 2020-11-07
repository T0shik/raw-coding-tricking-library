using System;
using System.Linq.Expressions;
using TrickingLibrary.Models.Moderation;

namespace TrickingLibrary.Api.ViewModels
{
    public static class ReviewViewModels
    {
        public static readonly Func<Review, object> Create = Projection.Compile();

        public static Expression<Func<Review, object>> Projection =>
            review => new
            {
                review.Id,
                review.ModerationItemId,
                review.Comment,
                review.Status,
            };

        public static readonly Func<Review, object> CreateWithUser = WithUserProjection.Compile();

        public static Expression<Func<Review, object>> WithUserProjection =>
            review => new
            {
                review.Id,
                review.ModerationItemId,
                review.Comment,
                review.Status,
                User = UserViewModels.CreateFlat(review.User),
            };
    }
}