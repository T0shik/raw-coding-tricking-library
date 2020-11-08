using FluentValidation;

namespace TrickingLibrary.Api.Form.Validation
{
    public class SubmissionFormValidation : AbstractValidator<SubmissionForm>
    {
        public SubmissionFormValidation()
        {
            RuleFor(x => x.TrickId).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Video).NotEmpty();
        }
    }
}