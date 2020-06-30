namespace TrickingLibrary.Models
{
    public class Submission : BaseModel
    {
        public int TrickId { get; set; }
        public string Video { get; set; }
        public string Description { get; set; }
    }
}