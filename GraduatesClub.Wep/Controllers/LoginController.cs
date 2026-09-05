using GraduatesClub.Domain.Security;
using GraduatesClub.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace GraduatesClub.Web.Controllers;
[AllowAnonymous]
public sealed class LoginController : Controller
{
    private readonly AppDbContext _db; public LoginController(AppDbContext db)=>_db=db;
    [HttpGet] public IActionResult Index()=>User.Identity?.IsAuthenticated==true?RedirectToAction("Index","Dashboard"):View();
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Index(string username,string password){var email=username.Trim().ToLowerInvariant();var user=await _db.UserProfiles.AsNoTracking().FirstOrDefaultAsync(x=>x.Email==email);if(user is null||!PasswordSecurity.Verify(password,user.PasswordHash)){ViewBag.Error="بيانات الدخول غير صحيحة.";return View();}var identity=new ClaimsIdentity(new[]{new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),new Claim(ClaimTypes.Name,user.FullName),new Claim(ClaimTypes.Email,user.Email)},CookieAuthenticationDefaults.AuthenticationScheme);await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(identity));return RedirectToAction("Index","Dashboard");}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Logout(){await HttpContext.SignOutAsync();return RedirectToAction(nameof(Index));}
}
