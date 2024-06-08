using BlogApp.Data.Concrete.EfCore;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly BlogContext _context;
        public HomeController(BlogContext context)
        {
            _context = context;

        }


        public IActionResult Index()
        {

            var claims = User.Claims;

            return View(
                new HomePageViewModel
                {
                    Posts       = _context.Posts.ToList(),
                    Settings    = _context.Settings.ToList(),
                }
               );
        }

        public async Task<IActionResult> List(string slug)
        {
            var posts = _context.Posts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(slug))
            {
                posts = posts.Where(i => i.Tags.Any(p => p.Url == slug));
            }

            return View(new HomePageViewModel
            {
                Posts = await posts.ToListAsync()
            });
        }


    }
}
