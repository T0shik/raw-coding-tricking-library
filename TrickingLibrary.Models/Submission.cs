namespace TrickingLibrary.Models
{
    public class Submission : BaseModel<int>
    {
        public string TrickId { get; set; }
        public string Video { get; set; }
        public string Description { get; set; }
    }
}