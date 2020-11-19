using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrickingLibrary.Api.Services.Email;

namespace TrickingLibrary.Api.Controllers
{
    [Route("/api/admin")]
    public class AdminController : ApiController
    {
        [HttpGet("moderators")]
        public async Task<IActionResult> ListModerators([FromServices] UserManager<IdentityUser> userManager)
        {
            var users = await userManager.GetUsersForClaimAsync(TrickingLibraryConstants.Claims.ModeratorClaim);

            return Ok(users.Select(x => new
            {
                x.Id,
                x.Email,
            }));
        }

        [HttpPost("moderators")]
        public async Task<IActionResult> InviteModerator(
            [FromBody] InviteModeratorForm form,
            [FromServices] EmailClient emailClient,
            [FromServices] UserManager<IdentityUser> userManager)
        {
            var existingUser = await userManager.FindByEmailAsync(form.Email);
            if (existingUser != null) return BadRequest("User with this email already exists");

            var moderator = new IdentityUser
            {
                UserName = form.Email,
                Email = form.Email,
            };

            var randomPart = new Random().Next(1000000000, int.MaxValue);
            var createResult = await userManager.CreateAsync(moderator, $"{randomPart}a1!A");
            if (!createResult.Succeeded)
            {
                var errorResponse = createResult.Errors
                    .Aggregate("Failed to create user:", (a, b) => $"${a} {b.Description}");
                return BadRequest(errorResponse);
            }

            await userManager.AddClaimAsync(moderator, TrickingLibraryConstants.Claims.ModeratorClaim);

            var code = await userManager.GeneratePasswordResetTokenAsync(moderator);

            var link = Url.Page("/Account/Moderator", "Get", new
            {
                email = form.Email,
                returnUrl = form.ReturnUrl,
                code,
            }, protocol: HttpContext.Request.Scheme);

            await emailClient.SendModeratorInviteAsync(form.Email, link);

            return Ok(link);
        }
    }

    public class InviteModeratorForm
    {
        public string Email { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class InviteModeratorFormValidation : AbstractValidator<InviteModeratorForm>
    {
        public InviteModeratorFormValidation()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.ReturnUrl).NotEmpty();
        }
    }
}