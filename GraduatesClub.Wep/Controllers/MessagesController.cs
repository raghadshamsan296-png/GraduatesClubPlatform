using GraduatesClub.Domain.Entities;
using GraduatesClub.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduatesClub.Web.Controllers;

public sealed class MessagesController : Controller
{
    private readonly AppDbContext _db;
    public MessagesController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index() => View(await _db.ContactMessages.AsNoTracking().OrderByDescending(x => x.SentAt).ToListAsync());

    public async Task<IActionResult> Details(int id)
    {
        var item = await _db.ContactMessages.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return item is null ? NotFound() : View(item);
    }

    [HttpGet]
    public IActionResult Create() => View(new ContactMessage());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContactMessage model)
    {
        if (!ModelState.IsValid) return View(model);
        model.Id = 0;
        model.FullName = model.FullName.Trim();
        model.Email = model.Email.Trim().ToLowerInvariant();
        model.Message = model.Message.Trim();
        model.SentAt = DateTime.UtcNow;
        _db.ContactMessages.Add(model);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _db.ContactMessages.FindAsync(id);
        return item is null ? NotFound() : View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ContactMessage model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);
        var item = await _db.ContactMessages.FindAsync(id);
        if (item is null) return NotFound();
        item.FullName = model.FullName.Trim();
        item.Email = model.Email.Trim().ToLowerInvariant();
        item.Message = model.Message.Trim();
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.ContactMessages.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return item is null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _db.ContactMessages.FindAsync(id);
        if (item is null) return NotFound();
        _db.ContactMessages.Remove(item);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
