using System.Linq;
using TrickingLibrary.Models.Abstractions;

namespace TrickingLibrary.Data.VersionMigrations
{
    public interface IEntityMigrationContext
    {
        IQueryable<VersionedModel> GetSource();
        void MigrateRelationships(int current, int target);
    }
}