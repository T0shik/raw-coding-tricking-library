using FluentValidation;

namespace TrickingLibrary.Api.Form.Validation
{
    public class UpdateDifficultyFormValidation : AbstractValidator<UpdateDifficultyForm>
    {
        public UpdateDifficultyFormValidation()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}