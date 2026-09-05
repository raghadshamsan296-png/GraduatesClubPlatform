using GraduatesClub.Domain.Entities;
using GraduatesClub.Infrastructure.Data;
using GraduatesClub.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduatesClub.Web.Controllers;

public sealed class JobController : Controller
{
    private readonly AppDbContext _context;
    public JobController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var jobs = await _context.Jobs.AsNoTracking().OrderByDescending(x => x.CreatedAt)
            .Select(x => new JobViewModel { Id = x.Id, JobTitle = x.JobTitle, CompanyName = x.CompanyName, Requirements = x.Requirements })
            .ToListAsync();
        return View(jobs);
    }

    [HttpGet]
    public IActionResult Create() => View();

    public async Task<IActionResult> Details(int id)
    {
        var x = await _context.Jobs.AsNoTracking().FirstOrDefaultAsync(j => j.Id == id);
        return x is null ? NotFound() : View(new JobViewModel
        {
            Id = x.Id, JobTitle = x.JobTitle, CompanyName = x.CompanyName, Requirements = x.Requirements
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(JobViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        _context.Jobs.Add(new JobPosting
        {
            JobTitle = model.JobTitle.Trim(), CompanyName = model.CompanyName.Trim(),
            Requirements = model.Requirements.Trim(), CreatedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet] public async Task<IActionResult> Edit(int id) { var x=await _context.Jobs.FindAsync(id); return x is null?NotFound():View(new JobViewModel{Id=x.Id,JobTitle=x.JobTitle,CompanyName=x.CompanyName,Requirements=x.Requirements}); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id,JobViewModel model){if(id!=model.Id)return BadRequest();if(!ModelState.IsValid)return View(model);var x=await _context.Jobs.FindAsync(id);if(x is null)return NotFound();x.JobTitle=model.JobTitle.Trim();x.CompanyName=model.CompanyName.Trim();x.Requirements=model.Requirements.Trim();await _context.SaveChangesAsync();return RedirectToAction(nameof(Index));}
    [HttpGet] public async Task<IActionResult> Delete(int id){var x=await _context.Jobs.AsNoTracking().FirstOrDefaultAsync(j=>j.Id==id);return x is null?NotFound():View(new JobViewModel{Id=x.Id,JobTitle=x.JobTitle,CompanyName=x.CompanyName,Requirements=x.Requirements});}
    [HttpPost,ActionName("Delete"),ValidateAntiForgeryToken] public async Task<IActionResult> DeleteConfirmed(int id){var x=await _context.Jobs.FindAsync(id);if(x is null)return NotFound();_context.Remove(x);await _context.SaveChangesAsync();return RedirectToAction(nameof(Index));}
}
