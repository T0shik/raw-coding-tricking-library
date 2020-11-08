using FluentValidation;

namespace TrickingLibrary.Api.Form.Validation
{
    public class CommentFormValidation : AbstractValidator<CommentCreationContext.CommentForm>
    {
        public CommentFormValidation()
        {
            RuleFor(x => x.ParentId).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();
        }
    }
}