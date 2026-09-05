using GraduatesClub.Domain.Entities;
using GraduatesClub.Infrastructure.Data;
using GraduatesClub.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduatesClub.Web.Controllers;

public sealed class AnnouncementController : Controller
{
    private readonly AppDbContext _context;
    public AnnouncementController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var items = await _context.Announcements.AsNoTracking()
            .OrderByDescending(x => x.Date)
            .Select(x => new AnnouncementViewModel { Id = x.Id, Title = x.Title, Content = x.Content, Date = x.Date })
            .ToListAsync();
        return View(items);
    }

    [HttpGet]
    public IActionResult Create() => View(new AnnouncementViewModel { Date = DateTime.Today });

    public async Task<IActionResult> Details(int id)
    {
        var x = await _context.Announcements.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        return x is null ? NotFound() : View(new AnnouncementViewModel
        {
            Id = x.Id, Title = x.Title, Content = x.Content, Date = x.Date
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AnnouncementViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        _context.Announcements.Add(new Announcement
        {
            Title = model.Title.Trim(), Content = model.Content.Trim(), Date = model.Date
        });
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet] public async Task<IActionResult> Edit(int id){var x=await _context.Announcements.FindAsync(id);return x is null?NotFound():View(new AnnouncementViewModel{Id=x.Id,Title=x.Title,Content=x.Content,Date=x.Date});}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id,AnnouncementViewModel model){if(id!=model.Id)return BadRequest();if(!ModelState.IsValid)return View(model);var x=await _context.Announcements.FindAsync(id);if(x is null)return NotFound();x.Title=model.Title.Trim();x.Content=model.Content.Trim();x.Date=model.Date;await _context.SaveChangesAsync();return RedirectToAction(nameof(Index));}
    [HttpGet] public async Task<IActionResult> Delete(int id){var x=await _context.Announcements.AsNoTracking().FirstOrDefaultAsync(a=>a.Id==id);return x is null?NotFound():View(new AnnouncementViewModel{Id=x.Id,Title=x.Title,Content=x.Content,Date=x.Date});}
    [HttpPost,ActionName("Delete"),ValidateAntiForgeryToken] public async Task<IActionResult> DeleteConfirmed(int id){var x=await _context.Announcements.FindAsync(id);if(x is null)return NotFound();_context.Remove(x);await _context.SaveChangesAsync();return RedirectToAction(nameof(Index));}
}
