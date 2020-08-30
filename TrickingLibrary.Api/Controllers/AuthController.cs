using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TrickingLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {

        [HttpGet("logout")]
        public async Task<IActionResult> Logout(string logoutId,
            [FromServices] SignInManager<IdentityUser> signInManager,
            [FromServices] IIdentityServerInteractionService interactionService)
        {
            await signInManager.SignOutAsync();

            var logoutContext = await interactionService.GetLogoutContextAsync(logoutId);

            if (string.IsNullOrEmpty(logoutContext.PostLogoutRedirectUri))
            {
                return BadRequest();
            }

            return Redirect(logoutContext.PostLogoutRedirectUri);
        }
    }
}