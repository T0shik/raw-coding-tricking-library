using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace TrickingLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [HttpGet("logout")]
        public async Task<IActionResult> Logout(string logoutId,
            [FromServices] SignInManager<IdentityUser> signInManager,
            [FromServices] IIdentityServerInteractionService interactionService,
            [FromServices] IWebHostEnvironment env)
        {
            await signInManager.SignOutAsync();

            if (string.IsNullOrEmpty(logoutId))
            {
                return Redirect(env.IsDevelopment() ? "https://localhost:3000/" : "/");
            }

            var logoutContext = await interactionService.GetLogoutContextAsync(logoutId);

            if (string.IsNullOrEmpty(logoutContext.PostLogoutRedirectUri))
            {
                return BadRequest();
            }

            return Redirect(logoutContext.PostLogoutRedirectUri);
        }
    }
}