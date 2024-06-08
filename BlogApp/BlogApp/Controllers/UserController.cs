using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entities;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class UserController : Controller
    {

        private readonly BlogContext _context;
        public UserController(BlogContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated) 
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var IsUser = await _context.Users.FirstOrDefaultAsync(i => i.UserEmail == model.UserEmail && i.Password == model.Password);

            if (ModelState.IsValid) 
            {
                if (IsUser != null)
                {
                    var userClaims = new List<Claim>();
                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, IsUser.UserId.ToString()));
                    userClaims.Add(new Claim(ClaimTypes.Name, IsUser.UserName ?? ""));
                    userClaims.Add(new Claim(ClaimTypes.GivenName, IsUser.Fullname ?? ""));
                    userClaims.Add(new Claim(ClaimTypes.UserData, IsUser.Image ?? ""));

                    if (IsUser.UserEmail == "info@info.com")
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }

                    //cookie varsa siler
                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    // otomatik beni hatırla
                    var LoginProperties = new AuthenticationProperties { IsPersistent = true };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity) , LoginProperties);

                    return RedirectToAction("Index", "Home");

                }

            }
            else
            {
                ModelState.AddModelError("", "Kullanıcı adı veya Parola Yanlış");
            }



            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                var users = _context.Users.FirstOrDefaultAsync(i => i.UserSurname == model.UserName || i.UserEmail == model.UserEmail);
                if (users == null) 
                {
                    var user = new User
                    {
                        UserName = model.UserName,
                        UserSurname = model.UserSurname,
                        Password = model.Password, // Ensure this is hashed before storing
                        UserEmail = model.UserEmail,
                        Image = "avatar.jpg"
                        // Map other properties if necessary
                    };
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Böyle bir kullanıcı var");
                }
                                
            }

            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}
