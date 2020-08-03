using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TrickingLibrary.Data;
using TrickingLibrary.Models.Moderation;

namespace TrickingLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/moderation-items")]
    public class ModerationItemController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public ModerationItemController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IEnumerable<ModerationItem> All() => _ctx.ModerationItems.ToList();

        [HttpGet("{id}")]
        public ModerationItem Get(int id) =>
            _ctx.ModerationItems.FirstOrDefault(x => x.Id.Equals(id));
    }
}