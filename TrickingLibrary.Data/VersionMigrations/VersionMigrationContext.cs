using System;
using System.Linq;
using TrickingLibrary.Models.Moderation;

namespace TrickingLibrary.Data.VersionMigrations
{
    public class VersionMigrationContext
    {
        private readonly AppDbContext _ctx;
        public VersionMigrationContext(AppDbContext ctx) => _ctx = ctx;

        private ModerationItem ModerationItem { get; set; }
        private IEntityMigrationContext EntityMigrationContext { get; set; }

        public VersionMigrationContext Setup(ModerationItem moderationItem)
        {
            ModerationItem = moderationItem;
            EntityMigrationContext = moderationItem.Type switch
            {
                ModerationTypes.Trick => new TrickMigrationContext(_ctx),
                ModerationTypes.Category => new CategoryMigrationContext(_ctx),
                _ => throw new ArgumentException(nameof(moderationItem.Type)),
            };
            return this;
        }

        public void Migrate()
        {
            // todo: throw some errors
            var source = EntityMigrationContext.GetSource();

            var current = source.FirstOrDefault(x => x.Id == ModerationItem.Current);
            var target = source.FirstOrDefault(x => x.Id == ModerationItem.Target);

            if (target == null)
            {
                throw new InvalidOperationException("Target not found");
            }

            if (current != null)
            {
                if (target.Version - current.Version <= 0)
                {
                    throw new InvalidVersionException($"Current Version is {current.Version}, Target version is {target.Version}, for {ModerationItem.Type}.");
                }

                current.Active = false;

                var outdatedModerationItems = _ctx.ModerationItems
                    .Where(x => !x.Deleted && x.Type == ModerationItem.Type && x.Id != ModerationItem.Id)
                    .ToList();

                foreach (var outdatedModItem in outdatedModerationItems)
                {
                    outdatedModItem.Current = target.Id;
                }
            }

            target.Active = true;
            EntityMigrationContext.MigrateRelationships(ModerationItem.Current, ModerationItem.Target);
        }

        public class InvalidVersionException : Exception
        {
            public InvalidVersionException(string message) : base(message)
            {
            }
        }
    }
}