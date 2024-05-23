namespace BlogApp.Entities
{
    public class Tag
    {
        public int TagId { get; set; }
        public string? Title { get; set; }
        public string? Url { get; set; }

        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
