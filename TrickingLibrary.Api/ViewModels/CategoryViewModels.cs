using System;
using System.Linq;
using System.Linq.Expressions;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.ViewModels
{
    public static class CategoryViewModels
    {
        public static readonly Func<Category, object> Create = Projection.Compile();

        public static Expression<Func<Category, object>> Projection =>
            category => new
            {
                category.Id,
                category.Name,
                category.Description,
                Tricks = category.Tricks.AsQueryable().Select(x => x.Trick.Slug).ToList(),
            };
    }
}