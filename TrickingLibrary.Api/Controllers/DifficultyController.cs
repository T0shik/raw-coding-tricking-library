using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public IEnumerable<Difficulty> All() => _ctx.Difficulties.ToList();

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
        public async Task<Difficulty> Create([FromBody] Difficulty category)
        {
            category.Id = category.Name.Replace(" ", "-").ToLowerInvariant();
            _ctx.Add(category);
            await _ctx.SaveChangesAsync();
            return category;
        }
    }
}