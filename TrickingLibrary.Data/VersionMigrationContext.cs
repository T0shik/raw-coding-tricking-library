using System;
using System.Linq;
using System.Threading.Tasks;
using TrickingLibrary.Models.Abstractions;
using TrickingLibrary.Models.Moderation;

namespace TrickingLibrary.Data
{
    public class VersionMigrationContext
    {
        private readonly AppDbContext _ctx;

        public VersionMigrationContext(AppDbContext ctx) => _ctx = ctx;

        public void Migrate(string targetId, int targetVersion, string targetType)
        {
            var (current, next) = ResolveCurrentAndNextEntities(targetId, targetVersion, targetType);

            if (current != null)
            {
                current.Active = false;
            }

            next.Active = true;
            next.Temporary = false;

            //todo roll id's of temporary versions
        }

        private (VersionedModel Current, VersionedModel Next) ResolveCurrentAndNextEntities(
            string targetId, int targetVersion, string targetType)
        {
            if (targetType == ModerationTypes.Trick)
            {
                var current = _ctx.Tricks.FirstOrDefault(x => x.Slug == targetId && x.Active);
                var next = _ctx.Tricks.FirstOrDefault(x => x.Slug == targetId && x.Version == targetVersion);
                return (current, next);
            }

            throw new ArgumentException(nameof(targetType));
        }
    }
}