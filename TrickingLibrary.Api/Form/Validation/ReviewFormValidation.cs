using FluentValidation;
using TrickingLibrary.Models.Moderation;

namespace TrickingLibrary.Api.Form.Validation
{
    public class ReviewFormValidation : AbstractValidator<ModerationItemReviewContext.ReviewForm>
    {
        public ReviewFormValidation()
        {
            RuleFor(x => x.Comment).NotEmpty().When(x => x.Status != ReviewStatus.Approved);
        }
    }
}