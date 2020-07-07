using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Models;

namespace TrickingLibrary.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Trick> Tricks { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<TrickRelationship> TrickRelationships { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TrickCategory> TrickCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TrickCategory>()
                        .HasKey(x => new {x.CategoryId, x.TrickId});

            modelBuilder.Entity<TrickRelationship>()
                        .HasKey(x => new {x.PrerequisiteId, x.ProgressionId});

            modelBuilder.Entity<TrickRelationship>()
                        .HasOne(x => x.Progression)
                        .WithMany(x => x.Prerequisites)
                        .HasForeignKey(x => x.ProgressionId);

            modelBuilder.Entity<TrickRelationship>()
                        .HasOne(x => x.Prerequisite)
                        .WithMany(x => x.Progressions)
                        .HasForeignKey(x => x.PrerequisiteId);
        }
    }
}