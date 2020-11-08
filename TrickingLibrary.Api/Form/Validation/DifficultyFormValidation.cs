using FluentValidation;

namespace TrickingLibrary.Api.Form.Validation
{
    public class DifficultyFormValidation : AbstractValidator<DifficultyForm>
    {
        public DifficultyFormValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}