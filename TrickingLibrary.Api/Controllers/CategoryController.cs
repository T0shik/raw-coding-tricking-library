using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public CategoryController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IEnumerable<Category> All() => _ctx.Categories.ToList();

        [HttpGet("{id}")]
        public Category Get(string id) =>
            _ctx.Categories
                .FirstOrDefault(x => x.Slug.Equals(id, StringComparison.InvariantCultureIgnoreCase));

        [HttpGet("{id}/tricks")]
        public IEnumerable<Trick> ListCategoryTricks(int id) =>
            _ctx.TrickCategories
                .Include(x => x.Trick)
                .Where(x => x.CategoryId == id)
                .Select(x => x.Trick)
                .ToList();

        [HttpPost]
        public async Task<Category> Create([FromBody] Category category)
        {
            category.Slug = category.Name.Replace(" ", "-").ToLowerInvariant();
            _ctx.Add(category);
            await _ctx.SaveChangesAsync();
            return category;
        }
    }
}