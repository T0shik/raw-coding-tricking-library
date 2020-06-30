namespace TrickingLibrary.Models
{
    public abstract class BaseModel
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
    }
}