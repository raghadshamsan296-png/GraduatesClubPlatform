using GraduatesClub.Domain.Entities;
using GraduatesClub.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using GraduatesClub.Domain.Security;

namespace GraduatesClub.Web.Controllers;

public sealed class SettingsController : Controller
{
    private readonly AppDbContext _context;
    public SettingsController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var profile = await _context.UserProfiles.AsNoTracking().OrderBy(x => x.Id).FirstOrDefaultAsync();
        return View(profile is null ? new UserProfileModel() : new UserProfileModel
        {
            Id = profile.Id, FullName = profile.FullName, Email = profile.Email, Phone = profile.Phone
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(UserProfileModel model)
    {
        if (!ModelState.IsValid) return View("Index", model);
        var profile = model.Id == 0 ? null : await _context.UserProfiles.FindAsync(model.Id);
        if (profile is null)
        {
            profile = new UserProfile();
            _context.UserProfiles.Add(profile);
        }
        profile.FullName = model.FullName.Trim();
        profile.Email = model.Email.Trim().ToLowerInvariant();
        profile.Phone = model.Phone?.Trim() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(model.Password)) profile.PasswordHash = PasswordSecurity.Hash(model.Password);
        try
        {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "تم حفظ بيانات الملف الشخصي بصورة دائمة.";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(nameof(model.Email), "البريد الإلكتروني مستخدم مسبقاً.");
            return View("Index", model);
        }
    }
}

public sealed class UserProfileModel
{
    public int Id { get; set; }
    [Required] public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    [MinLength(8, ErrorMessage = "كلمة المرور يجب ألا تقل عن 8 أحرف")]
    public string? Password { get; set; }
}
