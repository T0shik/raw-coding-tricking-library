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
    [Route("api/difficulties")]
    public class DifficultyController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public DifficultyController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IEnumerable<object> All() =>
            _ctx.Difficulties
                .Select(DifficultyViewModels.Projection)
                .ToList();

        [HttpGet("{id}")]
        public Difficulty Get(string id) =>
            _ctx.Difficulties
                .FirstOrDefault(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));

        [HttpGet("{id}/tricks")]
        public IEnumerable<Trick> ListDifficultyTricks(string id) =>
            _ctx.Tricks
                .Where(x => x.Difficulty.Equals(id, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DifficultyForm form)
        {
            _ctx.Add(new Difficulty
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