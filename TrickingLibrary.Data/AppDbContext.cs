using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Models;
using TrickingLibrary.Models.Moderation;

namespace TrickingLibrary.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Trick> Tricks { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<TrickRelationship> TrickRelationships { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TrickCategory> TrickCategories { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<ModerationItem> ModerationItems { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Trick>().HasKey(x => new {x.Slug, x.Version});
            modelBuilder.Entity<Category>().HasKey(x => new {x.Slug, x.Version});
            modelBuilder.Entity<Difficulty>().HasKey(x => new {x.Slug, x.Version});

            modelBuilder.Entity<TrickCategory>()
                .HasKey(x => new {x.CategoryId, x.CategoryVersion, x.TrickId, x.TrickVersion});

            modelBuilder.Entity<Trick>()
                .HasMany(x => x.TrickCategories)
                .WithOne(x => x.Trick)
                .HasForeignKey(x => new {x.TrickId, x.TrickVersion});

            modelBuilder.Entity<Category>()
                .HasMany(x => x.Tricks)
                .WithOne(x => x.Category)
                .HasForeignKey(x => new {x.CategoryId, x.CategoryVersion});

            modelBuilder.Entity<TrickRelationship>()
                .HasKey(x => new {x.PrerequisiteId, x.PrerequisiteVersion, x.ProgressionId, x.ProgressionVersion});

            modelBuilder.Entity<TrickRelationship>()
                .HasOne(x => x.Progression)
                .WithMany(x => x.Prerequisites)
                .HasForeignKey(x => new {x.ProgressionId, x.ProgressionVersion});

            modelBuilder.Entity<TrickRelationship>()
                .HasOne(x => x.Prerequisite)
                .WithMany(x => x.Progressions)
                .HasForeignKey(x => new {x.PrerequisiteId, x.PrerequisiteVersion});
        }
    }
}