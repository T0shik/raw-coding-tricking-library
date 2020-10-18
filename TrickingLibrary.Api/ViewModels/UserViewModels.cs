using System;
using System.Linq;
using System.Linq.Expressions;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.ViewModels
{
    public static class UserViewModels
    {
        public static readonly Func<User, object> CreateFlatCache = FlatProjection.Compile();
        public static object CreateFlat(User user) => CreateFlatCache(user);

        public static Expression<Func<User, object>> FlatProjection =>
            user => new
            {
                user.Id,
                user.Username,
                user.Image,
            };
    }
}