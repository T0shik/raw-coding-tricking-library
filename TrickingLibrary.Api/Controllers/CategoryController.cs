using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Api.Form;
using TrickingLibrary.Api.ViewModels;
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
        public IEnumerable<object> All() =>
            _ctx.Categories
                .Select(CategoryViewModels.Projection)
                .ToList();

        [HttpGet("{id}")]
        public Category Get(string id) =>
            _ctx.Categories
                .FirstOrDefault(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));

        [HttpGet("{id}/tricks")]
        public IEnumerable<Trick> ListCategoryTricks(string id) =>
            _ctx.TrickCategories
                .Include(x => x.Trick)
                .Where(x => x.CategoryId == id)
                .Select(x => x.Trick)
                .ToList();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryForm form)
        {
            _ctx.Add(new Category
            {
                Id = form.Name.Replace(" ", "-").ToLowerInvariant(),
                Name = form.Name,
                Description = form.Description,
            });
            await _ctx.SaveChangesAsync();
            return Ok();
        }
    }
}