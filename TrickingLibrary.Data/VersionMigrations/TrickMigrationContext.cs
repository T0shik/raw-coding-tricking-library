using System.Linq;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Models.Abstractions;

namespace TrickingLibrary.Data.VersionMigrations
{
    class TrickMigrationContext : IEntityMigrationContext
    {
        private readonly AppDbContext _ctx;

        public TrickMigrationContext(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public IQueryable<VersionedModel> GetSource()
        {
            return _ctx.Tricks;
        }

        public void MigrateRelationships(int current, int target)
        {
            if (current > 0)
            {
                var currentTrick = _ctx.Tricks
                    .Include(x => x.TrickDifficulties)
                    .Include(x => x.TrickCategories)
                    .Include(x => x.Prerequisites)
                    .Include(x => x.Progressions)
                    .FirstOrDefault(x => x.Id == current);

                foreach (var difficulty in currentTrick.TrickDifficulties)
                    difficulty.Active = false;
                foreach (var category in currentTrick.TrickCategories)
                    category.Active = false;
                foreach (var progression in currentTrick.Progressions)
                    progression.Active = false;
                foreach (var prerequisite in currentTrick.Prerequisites)
                    prerequisite.Active = false;
            }

            var targetTrick = _ctx.Tricks
                .Include(x => x.TrickDifficulties)
                .Include(x => x.TrickCategories)
                .Include(x => x.Prerequisites)
                .Include(x => x.Progressions)
                .FirstOrDefault(x => x.Id == target);

            foreach (var difficulty in targetTrick.TrickDifficulties)
                difficulty.Active = true;
            foreach (var category in targetTrick.TrickCategories)
                category.Active = true;
            foreach (var progression in targetTrick.Progressions)
                progression.Active = true;
            foreach (var prerequisite in targetTrick.Prerequisites)
                prerequisite.Active = true;
        }
    }
}