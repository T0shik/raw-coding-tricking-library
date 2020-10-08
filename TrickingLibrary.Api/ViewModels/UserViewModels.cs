using System;
using System.Linq;
using System.Linq.Expressions;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.ViewModels
{
    public static class UserViewModels
    {
        // public static readonly Func<User, object> CreateFlat = FlatProjection.Compile();
        public static object CreateFlat(User user) => FlatProjection.Compile().Invoke(user);

        public static Expression<Func<User, object>> FlatProjection =>
            user => new
            {
                user.Username,
                user.Image,
            };
    }
}