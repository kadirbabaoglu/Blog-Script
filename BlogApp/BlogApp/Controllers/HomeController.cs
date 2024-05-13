using BlogApp.Data.Concrete.EfCore;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogContext _context;
        public HomeController(ILogger<HomeController> logger, BlogContext context)
        {
            _context = context;

        }


        public IActionResult Index()
        {

            return View(
                new HomePageViewModel
                {
                    Posts       = _context.Posts.ToList(),
                    //Tags        = _context.Tags.ToList(),
                    Settings    = _context.Settings.ToList(),
                }
               );
        }

        
    }
}
