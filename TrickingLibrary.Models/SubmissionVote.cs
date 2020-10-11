using TrickingLibrary.Models.Abstractions;

namespace TrickingLibrary.Models
{
    public class SubmissionVote : BaseModel<int>
    {
        public string SubmissionId { get; set; }
        public Submission Submission { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}