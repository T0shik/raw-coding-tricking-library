using System;
using System.Linq;
using System.Linq.Expressions;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.ViewModels
{
    public static class DifficultyViewModels
    {
        public static readonly Func<Difficulty, object> CreateFlat = ProjectionFlat.Compile();

        public static Expression<Func<Difficulty, object>> ProjectionFlat =>
            difficulty => new
            {
                difficulty.Id,
                difficulty.Name,
                difficulty.Description,
                difficulty.Slug,
                difficulty.Version,
            };

        public static readonly Func<Difficulty, object> Create = Projection.Compile();

        public static Expression<Func<Difficulty, object>> Projection =>
            difficulty => new
            {
                difficulty.Id,
                difficulty.Name,
                difficulty.Description,
                difficulty.Slug,
                difficulty.Version,
                difficulty.State,
                Updated = difficulty.Updated.ToLocalTime().ToString("HH:mm dd/MM/yyyy"),
                Tricks = difficulty.Tricks.AsQueryable()
                    .Where(x => x.Active)
                    .Select(x => x.TrickId)
                    .ToList(),
            };
    }
}