using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;

namespace TrickingLibrary.Api.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected string UserId => GetClaim(ClaimTypes.NameIdentifier) ?? GetClaim(JwtClaimTypes.Subject);
        protected string Username => GetClaim(ClaimTypes.Name) ?? GetClaim(JwtClaimTypes.PreferredUserName);
        protected string Role => GetClaim(TrickingLibraryConstants.Claims.Role);

        private string GetClaim(string claimType) => User.Claims
            .FirstOrDefault(x => x.Type.Equals(claimType))?.Value;
    }
}