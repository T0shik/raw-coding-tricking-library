using System.Linq;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Models;
using TrickingLibrary.Models.Abstractions;

namespace TrickingLibrary.Data.VersionMigrations
{
    class CategoryMigrationContext : IEntityMigrationContext
    {
        private readonly AppDbContext _ctx;

        public CategoryMigrationContext(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public IQueryable<VersionedModel> GetSource()
        {
            return _ctx.Categories;
        }

        public void MigrateRelationships(int current, int target)
        {
            if (current > 0)
            {
                var relationships = _ctx.TrickCategories
                    .Where(x => x.CategoryId == current)
                    .ToList();

                foreach (var relationship in relationships)
                {
                    relationship.Active = false;
                    _ctx.Add(new TrickCategory
                    {
                        CategoryId = target,
                        TrickId = relationship.TrickId,
                        Active = true,
                    });
                }
            }
        }
    }
}