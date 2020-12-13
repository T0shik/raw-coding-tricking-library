using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TrickingLibrary.Api
{
    public class ApiIdentityDbContext : IdentityDbContext,
        IDataProtectionKeyContext
    {
        public ApiIdentityDbContext(DbContextOptions<ApiIdentityDbContext> options) : base(options)
        {

        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
    }
}