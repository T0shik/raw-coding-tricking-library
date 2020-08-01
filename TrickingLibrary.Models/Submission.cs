namespace TrickingLibrary.Models
{
    public class Submission : BaseModel<int>
    {
        public string TrickId { get; set; }
        public int VideoId { get; set; }
        public Video Video { get; set; }
        public bool VideoProcessed { get; set; }
        public string Description { get; set; }
    }
}