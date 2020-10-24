using TrickingLibrary.Models.Abstractions;

namespace TrickingLibrary.Models
{
    public class SubmissionMutable : Mutable
    {
        public int SubmissionId { get; set; }
        public Submission Submission { get; set; }
        public int Value { get; set; }
    }
}