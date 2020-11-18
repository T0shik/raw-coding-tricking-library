using FluentValidation;

namespace TrickingLibrary.Api.Form.Validation
{
    public class UpdateStagedTrickFormValidation : AbstractValidator<UpdateTrickForm>
    {
        public UpdateStagedTrickFormValidation()
        {
            RuleFor(x => x.Id).NotEqual(0);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Difficulty).NotEmpty();
            RuleFor(x => x.Categories).NotEmpty();
        }
    }
}