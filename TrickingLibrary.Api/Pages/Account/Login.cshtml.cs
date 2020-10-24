using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;

namespace TrickingLibrary.Api.Pages.Account
{
    public class Login : BasePage
    {
        [BindProperty] public LoginForm Form { get; set; }

        public void OnGet(string returnUrl)
        {
            Form = new LoginForm {ReturnUrl = returnUrl};
        }

        public async Task<IActionResult> OnPostAsync(
            [FromServices] SignInManager<IdentityUser> signInManager,
            [FromServices] IWebHostEnvironment env)
        {
            if (!ModelState.IsValid)
                return Page();

            var signInResult = await signInManager
                .PasswordSignInAsync(Form.Username, Form.Password, true, false);

            if (signInResult.Succeeded)
            {
                if (string.IsNullOrEmpty(Form.ReturnUrl))
                {
                    return Redirect(env.IsDevelopment() ? "https://localhost:3000/" : "/");
                }

                return Redirect(Form.ReturnUrl);
            }

            CustomErrors.Add("Invalid login attempt, please try again.");

            return Page();
        }

        public class LoginForm
        {
            public string ReturnUrl { get; set; }
            [Required] public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}