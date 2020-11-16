namespace TrickingLibrary.Models
{
    public class TrickDifficulty
    {
        public int TrickId { get; set; }
        public Trick Trick { get; set; }

        public int DifficultyId { get; set; }
        public Difficulty Difficulty { get; set; }
        public bool Active { get; set; }
    }
}