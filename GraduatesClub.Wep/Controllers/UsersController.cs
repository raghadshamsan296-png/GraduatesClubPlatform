using GraduatesClub.Domain.Entities;
using GraduatesClub.Domain.Security;
using GraduatesClub.Infrastructure.Data;
using GraduatesClub.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GraduatesClub.Web.Controllers;

public sealed class UsersController : Controller
{
    private readonly AppDbContext _db;
    public UsersController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index() => View(await _db.UserProfiles.AsNoTracking().OrderBy(x => x.FullName).ToListAsync());

    public async Task<IActionResult> Details(int id)
    {
        var item = await _db.UserProfiles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return item is null ? NotFound() : View(item);
    }

    [HttpGet]
    public IActionResult Create() => View(new UserProfileViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserProfileViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Password)) ModelState.AddModelError(nameof(model.Password), "كلمة المرور مطلوبة");
        if (!ModelState.IsValid) return View(model);
        _db.UserProfiles.Add(new UserProfile { FullName = model.FullName.Trim(), Email = model.Email.Trim().ToLowerInvariant(), Phone = model.Phone?.Trim() ?? string.Empty, PasswordHash = PasswordSecurity.Hash(model.Password!) });
        return await SaveOrShowEmailError(model, nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var x = await _db.UserProfiles.FindAsync(id);
        return x is null ? NotFound() : View(new UserProfileViewModel { Id = x.Id, FullName = x.FullName, Email = x.Email, Phone = x.Phone });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserProfileViewModel model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);
        var x = await _db.UserProfiles.FindAsync(id);
        if (x is null) return NotFound();
        x.FullName = model.FullName.Trim();
        x.Email = model.Email.Trim().ToLowerInvariant();
        x.Phone = model.Phone?.Trim() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(model.Password)) x.PasswordHash = PasswordSecurity.Hash(model.Password);
        return await SaveOrShowEmailError(model, nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.UserProfiles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return item is null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var currentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentId == id.ToString()) { TempData["ErrorMessage"] = "لا يمكنك حذف الحساب الذي تستخدمه الآن."; return RedirectToAction(nameof(Index)); }
        if (await _db.UserProfiles.CountAsync() <= 1) { TempData["ErrorMessage"] = "يجب إبقاء مستخدم واحد على الأقل."; return RedirectToAction(nameof(Index)); }
        var item = await _db.UserProfiles.FindAsync(id);
        if (item is null) return NotFound();
        _db.UserProfiles.Remove(item);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<IActionResult> SaveOrShowEmailError(UserProfileViewModel model, string action)
    {
        try { await _db.SaveChangesAsync(); return RedirectToAction(action); }
        catch (DbUpdateException) { ModelState.AddModelError(nameof(model.Email), "البريد الإلكتروني مستخدم مسبقاً."); return View(model); }
    }
}
