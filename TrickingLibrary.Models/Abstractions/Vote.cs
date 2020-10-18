namespace TrickingLibrary.Models.Abstractions
{
    public class Vote : BaseModel<int>
    {
        public string UserId { get; set; }
        public User User { get; set; }
    }
}