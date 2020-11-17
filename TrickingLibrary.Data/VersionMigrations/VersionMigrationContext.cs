using System;
using System.Linq;
using TrickingLibrary.Models;
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
            ModerationItem = moderationItem ?? throw new ArgumentException(nameof(moderationItem));
            EntityMigrationContext = moderationItem.Type switch
            {
                ModerationTypes.Trick => new TrickMigrationContext(_ctx),
                ModerationTypes.Category => new CategoryMigrationContext(_ctx),
                ModerationTypes.Difficulty => new DifficultyMigrationContext(_ctx),
                _ => throw new ArgumentException(nameof(moderationItem.Type)),
            };
            return this;
        }

        public void Migrate()
        {
            if (ModerationItem == null) throw new NullReferenceException(nameof(ModerationItem));
            if (EntityMigrationContext == null) throw new NullReferenceException(nameof(EntityMigrationContext));
            var source = EntityMigrationContext.GetSource();

            var current = source.FirstOrDefault(x => x.Id == ModerationItem.Current);
            var target = source.FirstOrDefault(x => x.Id == ModerationItem.Target);

            if (target == null)
            {
                throw new InvalidOperationException("Target not found");
            }

            if (current != null)
            {
                var newVersion = !current.Slug.Equals(target.Slug, StringComparison.InvariantCultureIgnoreCase);
                var outdatedVersion = target.Version - current.Version <= 0;
                if (outdatedVersion && !newVersion)
                {
                    throw new InvalidVersionException($"Current Version is {current.Version}, Target version is {target.Version}, for {ModerationItem.Type}.");
                }

                current.State = VersionState.Outdated;

                var outdatedModerationItems = _ctx.ModerationItems
                    .Where(x => !x.Deleted && x.Type == ModerationItem.Type && x.Id != ModerationItem.Id)
                    .ToList();

                foreach (var outdatedModItem in outdatedModerationItems)
                {
                    outdatedModItem.Current = target.Id;
                }
            }

            target.State = VersionState.Live;
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