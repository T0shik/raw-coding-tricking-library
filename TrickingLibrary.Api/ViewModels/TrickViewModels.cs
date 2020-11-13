using System;
using System.Linq;
using System.Linq.Expressions;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.ViewModels
{
    public static class TrickViewModels
    {
        public static readonly Func<Trick, object> Create = Projection.Compile();

        public static Expression<Func<Trick, object>> Projection =>
            trick => new
            {
                trick.Id,
                trick.Slug,
                trick.Name,
                trick.Description,
                trick.Difficulty,
                trick.Version,
                Categories = trick.TrickCategories
                    .AsQueryable()
                    .Select(x => x.CategoryId)
                    .ToList(),
                Prerequisites = trick.Prerequisites
                    .AsQueryable()
                    .Where(x => x.Active)
                    .Select(x => x.PrerequisiteId)
                    .ToList(),
                Progressions = trick.Progressions
                    .AsQueryable()
                    .Where(x => x.Active)
                    .Select(x => x.ProgressionId)
                    .ToList(),
            };

        public static readonly Func<Trick, object> CreateUser = UserProjection.Compile();
        public static Expression<Func<Trick, object>> UserProjection =>
            trick => new
            {
                trick.Id,
                trick.Slug,
                trick.Name,
                trick.Description,
                trick.Difficulty,
                trick.Version,
                Categories = trick.TrickCategories
                    .AsQueryable()
                    .Select(x => x.CategoryId)
                    .ToList(),
                Prerequisites = trick.Prerequisites
                    .AsQueryable()
                    .Where(x => x.Active)
                    .Select(x => x.PrerequisiteId)
                    .ToList(),
                Progressions = trick.Progressions
                    .AsQueryable()
                    .Where(x => x.Active)
                    .Select(x => x.ProgressionId)
                    .ToList(),
                User = UserViewModels.CreateFlat(trick.User),
            };

        public static readonly Func<Trick, object> CreateFull = FullProjection.Compile();
        public static Expression<Func<Trick, object>> FullProjection =>
            trick => new
            {
                trick.Id,
                trick.Slug,
                trick.Name,
                trick.Description,
                trick.Difficulty,
                trick.Version,
                Categories = trick.TrickCategories
                    .AsQueryable()
                    .Select(x => x.CategoryId)
                    .ToList(),
                Prerequisites = trick.Prerequisites
                    .AsQueryable()
                    .Select(x => x.PrerequisiteId)
                    .ToList(),
                Progressions = trick.Progressions
                    .AsQueryable()
                    .Select(x => x.ProgressionId)
                    .ToList(),
                User = UserViewModels.CreateFlat(trick.User),
            };

        public static readonly Func<Trick, object> CreateFlat = FlatProjection.Compile();
        public static Expression<Func<Trick, object>> FlatProjection =>
            trick => new
            {
                trick.Id,
                trick.Slug,
                trick.Name,
                trick.Description,
                trick.Difficulty,
                trick.Version,
            };
    }
}