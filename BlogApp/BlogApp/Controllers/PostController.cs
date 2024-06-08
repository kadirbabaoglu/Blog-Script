using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entities;
using BlogApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Linq;
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
            var detail = await _context
                .Posts
                        .Include(x => x.User)
                        .Include(x => x.Tags)
                        .Include(x => x.Comments)
                        .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Url == slug);
            return View(detail);
        }

        public ActionResult AddBlog()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddBlog(AddBlogViewModel model , IFormFile Image)
        {

            var allowedImageType = new[] { ".jpg", ".jpeg", ".png" };
            var extention = Path.GetExtension(Image.FileName).ToLower();
            var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extention}");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/postimage", randomFileName);

            if (Image != null)
            {
                if (!allowedImageType.Contains(extention))
                {
                    ModelState.AddModelError("", "Lütfen geçerli bir resim türü seçiniz!");
                }
            }

            if (ModelState.IsValid)
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await Image!.CopyToAsync(stream);

                }
                model.Image = randomFileName;

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var post = new Post
                {
                    Title = model.Title,
                    Url = SeoUrlHelper.ToSeoFriendlyUrl(model.Title),
                    Description = model.Description,
                    Image = randomFileName,
                    IsActive = false,
                    PublishedOn = DateTime.Now,
                    UserId = int.Parse(userId ?? "")
                };

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index" , "Home");
            }


            return View();
        }


        [HttpPost]
        public JsonResult AddComment(int PostId,  string CommentText)
        
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var avatar = User.FindFirstValue(ClaimTypes.UserData);

            var data = new Comment
            {
                PostId = PostId,
                CommentText = CommentText,
                PublishedOn = DateTime.Now,
                UserId = int.Parse(userId)
            };

            _context.Comments.Add(data);
            _context.SaveChanges();

            var result = new
            {
               user = username,
               text = CommentText,
               date = DateTime.Now,
               image = avatar

            };

            return new JsonResult ( result );


        }

    }
}
