using FluentValidation;

namespace TrickingLibrary.Api.Form.Validation
{
    public class CreateDifficultyFormValidation : AbstractValidator<CreateDifficultyForm>
    {
        public CreateDifficultyFormValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}