using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BlogContext>(options =>
{
    var config = builder.Configuration;
    var connecitonString = config.GetConnectionString("MySqlConnection");
    var version = new MySqlServerVersion(new Version(8,2,0));
    options.UseMySql(connecitonString , version);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opt =>
{
    opt.LoginPath = "/User/Login";
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "post_details",
    pattern: "posts/{slug}",
    defaults: new{ controller = "Post", action = "Details" });

app.MapControllerRoute(
    name: "post_bt_tagname",
    pattern: "posts/tag/{slug}",
    defaults: new { controller = "Home", action = "List" });

app.MapControllerRoute(
    name: "user_profil",
    pattern: "profil/{username}",
    defaults: new { controller = "User", action = "Profil" });


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
