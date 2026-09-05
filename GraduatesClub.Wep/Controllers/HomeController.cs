using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GraduatesClub.Web.Models;
using GraduatesClub.Infrastructure.Data;
using GraduatesClub.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace GraduatesClub.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(new PublicHomeViewModel
            {
                AlumniCount = await _context.Alumni.CountAsync(),
                DepartmentCount = await _context.Departments.CountAsync(),
                UpcomingEvents = await _context.Events.AsNoTracking().Where(x => x.EventDate >= DateTime.Today).OrderBy(x => x.EventDate).Take(3).ToListAsync(),
                LatestJobs = await _context.Jobs.AsNoTracking().OrderByDescending(x => x.CreatedAt).Take(3).ToListAsync(),
                LatestAnnouncements = await _context.Announcements.AsNoTracking().OrderByDescending(x => x.Date).Take(3).ToListAsync()
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous, HttpGet]
        public IActionResult Contact() => View(new ContactViewModel());

        [AllowAnonymous, HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            _context.ContactMessages.Add(new ContactMessage
            {
                FullName = model.FullName.Trim(), Email = model.Email.Trim().ToLowerInvariant(),
                Message = model.Message.Trim(), SentAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "تم إرسال رسالتك وحفظها بنجاح.";
            return RedirectToAction(nameof(Contact));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public sealed class PublicHomeViewModel
    {
        public int AlumniCount { get; set; }
        public int DepartmentCount { get; set; }
        public List<ClubEvent> UpcomingEvents { get; set; } = new();
        public List<JobPosting> LatestJobs { get; set; } = new();
        public List<Announcement> LatestAnnouncements { get; set; } = new();
    }
}
