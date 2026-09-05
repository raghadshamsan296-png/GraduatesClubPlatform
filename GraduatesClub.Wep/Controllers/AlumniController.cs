using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GraduatesClub.Web.Models;
using GraduatesClub.Web.ViewModels;
using GraduatesClub.Infrastructure.Data;
using GraduatesClub.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GraduatesClub.Web.Controllers
{
    public class AlumniController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

        public AlumniController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var alumniList = await _context.Alumni.Include(x => x.Department).AsNoTracking().ToListAsync();
            return View(alumniList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDepartmentsAsync();
            return View(new AlumniViewModel { GraduationYear = DateTime.Today.Year });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AlumniViewModel model)
        {
            ValidatePhoto(model.Photo);
            if (ModelState.IsValid)
            {
                var alumni = new Alumni
                {
                    FullName = model.FullName.Trim(),
                    Email = model.Email.Trim().ToLowerInvariant(),
                    Phone = model.Phone?.Trim() ?? string.Empty,
                    GraduationYear = model.GraduationYear,
                    DepartmentId = model.DepartmentId,
                    PhotoPath = await SavePhotoAsync(model.Photo)
                };
                _context.Alumni.Add(alumni);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(nameof(model.Email), "البريد الإلكتروني مستخدم مسبقاً.");
                }
            }
            await LoadDepartmentsAsync(model.DepartmentId);
            return View(model);
        }

        private async Task LoadDepartmentsAsync(int? selectedId = null)
        {
            var departments = await _context.Departments.AsNoTracking().OrderBy(x => x.Name).ToListAsync();
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name", selectedId);
        }

        public async Task<IActionResult> Details(int id){var x=await _context.Alumni.Include(a=>a.Department).AsNoTracking().FirstOrDefaultAsync(a=>a.Id==id);return x is null?NotFound():View(x);}
        [HttpGet] public async Task<IActionResult> Edit(int id){var x=await _context.Alumni.FindAsync(id);if(x is null)return NotFound();await LoadDepartmentsAsync(x.DepartmentId);return View(new AlumniViewModel{Id=x.Id,FullName=x.FullName,Email=x.Email,Phone=x.Phone,GraduationYear=x.GraduationYear,DepartmentId=x.DepartmentId,ExistingPhotoPath=x.PhotoPath});}
        [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id,AlumniViewModel model){if(id!=model.Id)return BadRequest();ValidatePhoto(model.Photo);if(!ModelState.IsValid){await LoadDepartmentsAsync(model.DepartmentId);return View(model);}var x=await _context.Alumni.FindAsync(id);if(x is null)return NotFound();x.FullName=model.FullName.Trim();x.Email=model.Email.Trim().ToLowerInvariant();x.Phone=model.Phone?.Trim()??string.Empty;x.GraduationYear=model.GraduationYear;x.DepartmentId=model.DepartmentId;if(model.Photo is not null){DeletePhoto(x.PhotoPath);x.PhotoPath=await SavePhotoAsync(model.Photo);}try{await _context.SaveChangesAsync();return RedirectToAction(nameof(Index));}catch(DbUpdateException){ModelState.AddModelError(nameof(model.Email),"البريد الإلكتروني مستخدم مسبقاً.");model.ExistingPhotoPath=x.PhotoPath;await LoadDepartmentsAsync(model.DepartmentId);return View(model);}}
        [HttpGet] public async Task<IActionResult> Delete(int id){var x=await _context.Alumni.AsNoTracking().FirstOrDefaultAsync(a=>a.Id==id);return x is null?NotFound():View(x);}
        [HttpPost,ActionName("Delete"),ValidateAntiForgeryToken] public async Task<IActionResult> DeleteConfirmed(int id){var x=await _context.Alumni.FindAsync(id);if(x is null)return NotFound();_context.Remove(x);try{await _context.SaveChangesAsync();DeletePhoto(x.PhotoPath);return RedirectToAction(nameof(Index));}catch(DbUpdateException){TempData["ErrorMessage"]="لا يمكن حذف خريج مرتبط بفعاليات.";return RedirectToAction(nameof(Index));}}

        private void ValidatePhoto(IFormFile? photo)
        {
            if (photo is null) return;
            if (photo.Length > 2 * 1024 * 1024)
                ModelState.AddModelError("Photo", "حجم الصورة يجب ألا يتجاوز 2 ميجابايت.");
            var extension = Path.GetExtension(photo.FileName).ToLowerInvariant();
            if (!AllowedImageExtensions.Contains(extension))
                ModelState.AddModelError("Photo", "الصيغ المسموحة: JPG وPNG وWEBP.");
        }

        private async Task<string?> SavePhotoAsync(IFormFile? photo)
        {
            if (photo is null || photo.Length == 0) return null;
            var folder = Path.Combine(_environment.WebRootPath, "uploads", "alumni");
            Directory.CreateDirectory(folder);
            var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(photo.FileName).ToLowerInvariant()}";
            await using var stream = System.IO.File.Create(Path.Combine(folder, fileName));
            await photo.CopyToAsync(stream);
            return $"/uploads/alumni/{fileName}";
        }

        private void DeletePhoto(string? photoPath)
        {
            if (string.IsNullOrWhiteSpace(photoPath)) return;
            var relativePath = photoPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(_environment.WebRootPath, relativePath);
            if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
        }
    }
}
