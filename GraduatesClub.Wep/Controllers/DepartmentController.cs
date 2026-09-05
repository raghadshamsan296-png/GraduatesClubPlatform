using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GraduatesClub.Web.Models;
using GraduatesClub.Infrastructure.Data;

namespace GraduatesClub.Web.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly AppDbContext _context;

        public DepartmentController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments.ToListAsync();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Domain.Entities.Department department)
        {
            department.Name = department.Name?.Trim() ?? string.Empty;
            if (department.Name.Length < 2)
                ModelState.AddModelError(nameof(department.Name), "اسم القسم يجب أن يتكون من حرفين على الأقل.");

            if (ModelState.IsValid)
            {
                _context.Departments.Add(department);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(nameof(department.Name), "اسم القسم مستخدم مسبقاً.");
                }
            }
            return View(department);
        }

        public async Task<IActionResult> Details(int id)
        {
            var department = await _context.Departments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return department is null ? NotFound() : View(department);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            return department is null ? NotFound() : View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Domain.Entities.Department department)
        {
            if (id != department.Id)
                return BadRequest();

            department.Name = department.Name?.Trim() ?? string.Empty;
            if (department.Name.Length < 2)
                ModelState.AddModelError(nameof(department.Name), "اسم القسم يجب أن يتكون من حرفين على الأقل.");

            if (!ModelState.IsValid)
                return View(department);

            _context.Update(department);
            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(nameof(department.Name), "اسم القسم مستخدم مسبقاً.");
                return View(department);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var department = await _context.Departments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return department is null ? NotFound() : View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department is null)
                return NotFound();

            _context.Departments.Remove(department);
            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "لا يمكن حذف قسم مرتبط بخريجين.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
