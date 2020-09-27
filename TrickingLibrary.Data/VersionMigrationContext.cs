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

        public void Migrate(ModerationItem modItem)
        {
            var source = GetSource(modItem.Type);

            var current = source.FirstOrDefault(x => x.Id == modItem.Current);
            var target = source.FirstOrDefault(x => x.Id == modItem.Target);

            if (target == null)
            {
                throw new InvalidOperationException("Target not found");
            }

            if (current != null)
            {
                if (target.Version - current.Version <= 0)
                {
                    throw new InvalidVersionException($"Current Version is {current.Version}, Target version is {target.Version}, for {modItem.Type}.");
                }

                current.Active = false;

                var outdatedModerationItems = _ctx.ModerationItems
                    .Where(x => !x.Deleted && x.Type == modItem.Type && x.Id != modItem.Id)
                    .ToList();

                foreach (var outdatedModItem in outdatedModerationItems)
                {
                    outdatedModItem.Current = target.Id;
                }
            }

            target.Active = true;
        }


        private IQueryable<VersionedModel> GetSource(string type)
        {
            if (type == ModerationTypes.Trick)
            {
                return _ctx.Tricks;
            }

            throw new ArgumentException(nameof(type));
        }

        public class InvalidVersionException : Exception
        {
            public InvalidVersionException(string message) : base(message){ }
        }
    }
}