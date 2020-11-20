using TrickingLibrary.Models.Abstractions;

namespace TrickingLibrary.Models
{
    public class Video : BaseModel<int>
    {
        public int? SubmissionId { get; set; }
        public Submission Submission { get; set; }
        public string VideoLink { get; set; }
        public string ThumbLink { get; set; }
    }
}