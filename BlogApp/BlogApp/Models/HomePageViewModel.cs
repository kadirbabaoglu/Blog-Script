using BlogApp.Entities;

namespace BlogApp.Models
{
    public class HomePageViewModel
    {
        public List<Post> Posts { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Setting> Settings { get; set; }
        public List<About> Abouts { get; set; }
        public List<Comment> Comments { get; set; }
        public List<User> Users { get; set; }


    }
}
