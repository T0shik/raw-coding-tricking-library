using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Api.Form;
using TrickingLibrary.Api.ViewModels;
using TrickingLibrary.Data;
using TrickingLibrary.Models;
using TrickingLibrary.Models.Moderation;

namespace TrickingLibrary.Api.Controllers
{
    [Route("api/categories")]
    public class CategoryController : ApiController
    {
        private readonly AppDbContext _ctx;

        public CategoryController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IEnumerable<object> All() =>
            _ctx.Categories
                .Where(x => !x.Deleted && x.Active)
                .Select(CategoryViewModels.Projection)
                .ToList();

        [HttpGet("{value}")]
        public IActionResult Get(string value)
        {
            var category = _ctx.Categories
                .WhereIdOrSlug(value)
                .Select(CategoryViewModels.Projection)
                .FirstOrDefault();

            if (category == null)
            {
                return NoContent();
            }

            return Ok(category);
        }

        [HttpGet("{id}/tricks")]
        public IEnumerable<Trick> ListCategoryTricks(int id) =>
            _ctx.TrickCategories
                .Include(x => x.Trick)
                .Where(x => x.CategoryId == id)
                .Select(x => x.Trick)
                .ToList();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryForm form)
        {
            var category = new Category
            {
                Slug = form.Name.Replace(" ", "-").ToLowerInvariant(),
                Name = form.Name,
                Description = form.Description,
                Version = 1,
                UserId = UserId,
            };
            _ctx.Add(category);
            await _ctx.SaveChangesAsync();

            _ctx.ModerationItems.Add(new ModerationItem
            {
                Target = category.Id,
                UserId = UserId,
                Type = ModerationTypes.Category,
            });
            await _ctx.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryForm form)
        {
            var category = _ctx.Categories.FirstOrDefault(x => x.Id == form.Id);

            if (category == null)
            {
                return NoContent();
            }

            var newCategory = new Category
            {
                Slug = form.Name.Replace(" ", "-").ToLowerInvariant(),
                Name = form.Name,
                Description = form.Description,
                Version = category.Version + 1,
                UserId = UserId,
            };

            _ctx.Add(newCategory);
            await _ctx.SaveChangesAsync();

            _ctx.ModerationItems.Add(new ModerationItem
            {
                Current = category.Id,
                Target = newCategory.Id,
                UserId = UserId,
                Type = ModerationTypes.Category,
            });
            await _ctx.SaveChangesAsync();

            return Ok();
        }
    }
}