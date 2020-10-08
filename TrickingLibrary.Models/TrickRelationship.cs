namespace TrickingLibrary.Models
{
    public class TrickRelationship
    {
        public int PrerequisiteId { get; set; }
        public Trick Prerequisite { get; set; }
        public int ProgressionId { get; set; }
        public Trick Progression { get; set; }
        public bool Active { get; set; }
    }
}