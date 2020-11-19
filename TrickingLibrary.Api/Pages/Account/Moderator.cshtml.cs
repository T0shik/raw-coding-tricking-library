using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TrickingLibrary.Api.Pages.Account
{
    public class Moderator : BasePage
    {
        [BindProperty] public ModeratorRegisterForm Form { get; set; }

        public void OnGet(string code, string email, string returnUrl)
        {
            Form = new ModeratorRegisterForm
            {
                Email = email,
                Code = code,
                ReturnUrl = returnUrl,
            };
        }

        public async Task<IActionResult> OnPostAsync(
            [FromServices] UserManager<IdentityUser> userManager,
            [FromServices] SignInManager<IdentityUser> signInManager)
        {
            if (!ModelState.IsValid)
                return Page();

            var existingUser = await userManager.FindByNameAsync(Form.Username);
            if (existingUser != null)
            {
                CustomErrors.Add("Username already taken.");
                return Page();
            }

            var user = await userManager.FindByNameAsync(Form.Email);
            var resetPasswordResult = await userManager.ResetPasswordAsync(user, Form.Code, Form.Password);
            if (resetPasswordResult.Succeeded)
            {
                user.UserName = Form.Username;
                await userManager.UpdateAsync(user);

                await signInManager.SignInAsync(user, true);

                return Redirect(Form.ReturnUrl);
            }

            CustomErrors.Add("Failed to create account.");

            return Page();
        }

        public class ModeratorRegisterForm
        {
            [Required] public string ReturnUrl { get; set; }
            [Required] public string Email { get; set; }
            [Required] public string Code { get; set; }
            [Required] public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare(nameof(Password))]
            public string ConfirmPassword { get; set; }
        }
    }
}