using TrickingLibrary.Models.Abstractions;

namespace TrickingLibrary.Models
{
    public class SubmissionVote : Mutable
    {
        public int SubmissionId { get; set; }
        public Submission Submission { get; set; }
        public int Value { get; set; }
    }
}