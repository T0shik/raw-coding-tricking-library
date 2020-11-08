using FluentValidation;

namespace TrickingLibrary.Api.Form.Validation
{
    public class CreateTrickFormValidation : AbstractValidator<CreateTrickForm>
    {
        public CreateTrickFormValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Difficulty).NotEmpty();
            RuleFor(x => x.Categories).NotEmpty();
        }
    }
}