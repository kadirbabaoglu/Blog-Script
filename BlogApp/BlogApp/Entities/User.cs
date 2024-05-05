namespace BlogApp.Entities
{
    public class User
    {   
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserSurname { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhone { get; set; }
        public string? Image { get; set; }
        public string? ImageUrl { get{ return this.UserName + " " + this.UserSurname; }}

        public List<Post> Posts { get; set; } = new List<Post>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }

    
}
