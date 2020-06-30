using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Models;

namespace TrickingLibrary.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Trick> Tricks { get; set; }
        public DbSet<Submission> Submissions { get; set; }
    }
}