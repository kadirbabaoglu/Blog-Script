using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlogApp.Controllers
{
    public class PostController : Controller
    {

        private readonly BlogContext _context;

        public PostController(BlogContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View(_context.Posts.ToList());
        }

        public async Task<ActionResult> Details(string slug)
        {
            var detail = await _context.Posts.Include(x => x.Tags).Include(x => x.Comments).ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.Url == slug);
            return View(detail);
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public JsonResult AddComment(int PostId, string UserName, string CommentText)
        
        {
            
            
            var data = new Comment
            {
                PostId = PostId,
                CommentText = CommentText,
                PublishedOn = DateTime.Now,
                User = new User { UserName = UserName, Image = "avatar.jpg" }
            };

            _context.Comments.Add(data);
            _context.SaveChanges();

            var result = new
            {
               user = UserName,
               text = CommentText,
               date = DateTime.Now,
               image = data.User.Image,

            };

            return new JsonResult ( result );


        }

    }
}
