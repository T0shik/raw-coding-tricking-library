using FluentValidation;

namespace TrickingLibrary.Api.Form.Validation
{
    public class UpdateTrickFormValidation : AbstractValidator<UpdateTrickForm>
    {
        public UpdateTrickFormValidation()
        {
            RuleFor(x => x.Id).NotEqual(0);
            RuleFor(x => x.Reason).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Difficulty).NotEmpty();
            RuleFor(x => x.Categories).NotEmpty();
        }
    }
}