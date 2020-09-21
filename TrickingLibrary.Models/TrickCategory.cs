namespace TrickingLibrary.Models
{
    public class TrickCategory
    {
        public string TrickId { get; set; }
        public int TrickVersion { get; set; }
        public Trick Trick { get; set; }

        public string CategoryId { get; set; }
        public int CategoryVersion { get; set; }

        public Category Category { get; set; }
    }
}