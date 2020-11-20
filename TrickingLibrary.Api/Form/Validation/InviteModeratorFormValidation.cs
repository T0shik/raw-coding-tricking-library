using FluentValidation;

namespace TrickingLibrary.Api.Controllers
{
    public class InviteModeratorFormValidation : AbstractValidator<InviteModeratorForm>
    {
        public InviteModeratorFormValidation()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.ReturnUrl).NotEmpty();
        }
    }
}