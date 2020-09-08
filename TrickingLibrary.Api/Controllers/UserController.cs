using Microsoft.AspNetCore.Mvc;

namespace TrickingLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        [HttpGet("me")]
        public IActionResult GetMe() => Ok();

        [HttpGet("{id}")]
        public IActionResult GetUser(string id) => Ok();
    }
}