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
    [Route("api/tricks")]
    public class TricksController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public TricksController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IEnumerable<Trick> All() => _ctx.Tricks.ToList();

        [HttpGet("{id}")]
        public Trick Get(int id) => _ctx.Tricks.FirstOrDefault(x => x.Id.Equals(id));

        [HttpGet("{trickId}/submissions")]
        public IEnumerable<Submission> ListSubmissionsForTrick(int trickId) =>
            _ctx.Submissions.Where(x => x.TrickId.Equals(trickId)).ToList();

        [HttpPost]
        public async Task<Trick> Create([FromBody] Trick trick)
        {
           _ctx.Add(trick);
           await _ctx.SaveChangesAsync();
           return trick;
        }

        [HttpPut]
        public async Task<Trick> Update([FromBody] Trick trick)
        {
            if (trick.Id == 0)
            {
                return null;
            }

            _ctx.Add(trick);
            await _ctx.SaveChangesAsync();
            return trick;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var trick = _ctx.Tricks.FirstOrDefault(x => x.Id.Equals(id));
            trick.Deleted = true;
            await _ctx.SaveChangesAsync();
            return Ok();
        }
    }
}