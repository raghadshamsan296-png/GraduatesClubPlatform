using GraduatesClub.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace GraduatesClub.Web.Controllers;
public sealed class DashboardController : Controller
{
    private readonly AppDbContext _db;public DashboardController(AppDbContext db)=>_db=db;
    public async Task<IActionResult> Index()=>View(new DashboardViewModel{Alumni=await _db.Alumni.CountAsync(),Departments=await _db.Departments.CountAsync(),Jobs=await _db.Jobs.CountAsync(),Events=await _db.Events.CountAsync(),Announcements=await _db.Announcements.CountAsync(),Messages=await _db.ContactMessages.CountAsync()});
}
public sealed class DashboardViewModel{public int Alumni{get;set;}public int Departments{get;set;}public int Jobs{get;set;}public int Events{get;set;}public int Announcements{get;set;}public int Messages{get;set;}}
