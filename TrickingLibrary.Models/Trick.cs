using System.Collections.Generic;

namespace TrickingLibrary.Models
{
    public class Trick : BaseModel<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DifficultyId { get; set; }
        // todo do we need this?
        public Difficulty Difficulty { get; set; }

        public IList<TrickRelationship> Prerequisites { get; set; }
        public IList<TrickRelationship> Progressions { get; set; }

        public IList<TrickCategory> TrickCategories { get; set; }
    }

    public class TrickCategory
    {
        public string TrickId { get; set; }
        public Trick Trick { get; set; }

        public string CategoryId { get; set; }
        public Category Category { get; set; }
    }

    public class Category : BaseModel<string>
    {
        public string Description { get; set; }
        public IList<TrickCategory> Tricks { get; set; }
    }

    public class TrickRelationship
    {
        public string PrerequisiteId { get; set; }
        public Trick Prerequisite { get; set; }
        public string ProgressionId { get; set; }
        public Trick Progression { get; set; }
    }

    public class Difficulty : BaseModel<string>
    {
        public string Description { get; set; }
        public IList<Trick> Tricks { get; set; }
    }
}