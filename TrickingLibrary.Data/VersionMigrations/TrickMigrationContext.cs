using System.Linq;
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
                _ctx.TrickRelationships
                    .Where(x => x.PrerequisiteId == current || x.ProgressionId == current)
                    .ToList()
                    .ForEach(x => x.Active = false);
            }

            _ctx.TrickRelationships
                .Where(x => x.PrerequisiteId == target || x.ProgressionId == target)
                .ToList()
                .ForEach(x => x.Active = true);
        }
    }
}