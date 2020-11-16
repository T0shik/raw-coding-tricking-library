using System;
using System.Linq;
using TrickingLibrary.Models;
using TrickingLibrary.Models.Abstractions;

namespace TrickingLibrary.Data.VersionMigrations
{
    public class DifficultyMigrationContext : IEntityMigrationContext
    {
        private readonly AppDbContext _ctx;

        public DifficultyMigrationContext(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public IQueryable<VersionedModel> GetSource()
        {
            return _ctx.Difficulties;
        }

        public void MigrateRelationships(int current, int target)
        {
            if (current > 0)
            {
                var relationships = _ctx.TrickDifficulties
                    .Where(x => x.DifficultyId == current)
                    .ToList();

                foreach (var relationship in relationships)
                {
                    relationship.Active = false;
                    _ctx.Add(new TrickDifficulty
                    {
                        DifficultyId = target,
                        TrickId = relationship.TrickId,
                        Active = true,
                    });
                }
            }
        }
    }
}