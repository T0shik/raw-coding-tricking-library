namespace TrickingLibrary.Data
{
    public class FeedQuery
    {
        public string Order { get; set; }
        public int Cursor { get; set; }
        public int Limit { get; } = 10;
    }
}