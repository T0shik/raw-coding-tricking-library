using TrickingLibrary.Models.Moderation;

namespace TrickingLibrary.Api.Form
{
    public class ReviewForm
    {
        public string Comment { get; set; }
        public ReviewStatus Status { get; set; }
    }
}