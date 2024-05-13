using BlogApp.Data.Concrete.EfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.ViewComponents
{
    public class TagsMenu : ViewComponent
    {
        private readonly BlogContext _context;

        public TagsMenu(BlogContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View( _context.Tags.ToList());
        }
    }
}
