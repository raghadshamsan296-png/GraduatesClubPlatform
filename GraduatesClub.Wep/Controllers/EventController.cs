using GraduatesClub.Domain.Entities;
using GraduatesClub.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GraduatesClub.Web.Controllers;

public sealed class EventController : Controller
{
    private readonly AppDbContext _context;
    public EventController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var events = await _context.Events.Include(x => x.Alumni).AsNoTracking()
            .OrderByDescending(x => x.EventDate)
            .Select(x => new EventViewModel { Id=x.Id,Title=x.Title,Description=x.Description,EventDate=x.EventDate,AlumniId=x.AlumniId,AlumniName=x.Alumni!.FullName })
            .ToListAsync();
        return View(events);
    }

    [HttpGet] public async Task<IActionResult> Create(){await LoadAlumniAsync();return View(new EventViewModel{EventDate=DateTime.Today});}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(EventViewModel model){if(!ModelState.IsValid){await LoadAlumniAsync(model.AlumniId);return View(model);}_context.Events.Add(new ClubEvent{Title=model.Title.Trim(),Description=model.Description.Trim(),EventDate=model.EventDate,AlumniId=model.AlumniId});await _context.SaveChangesAsync();return RedirectToAction(nameof(Index));}
    public async Task<IActionResult> Details(int id){var x=await FindModelAsync(id);return x is null?NotFound():View(x);}
    [HttpGet] public async Task<IActionResult> Edit(int id){var x=await FindModelAsync(id);if(x is null)return NotFound();await LoadAlumniAsync(x.AlumniId);return View(x);}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id,EventViewModel model){if(id!=model.Id)return BadRequest();if(!ModelState.IsValid){await LoadAlumniAsync(model.AlumniId);return View(model);}var x=await _context.Events.FindAsync(id);if(x is null)return NotFound();x.Title=model.Title.Trim();x.Description=model.Description.Trim();x.EventDate=model.EventDate;x.AlumniId=model.AlumniId;await _context.SaveChangesAsync();return RedirectToAction(nameof(Index));}
    [HttpGet] public async Task<IActionResult> Delete(int id){var x=await FindModelAsync(id);return x is null?NotFound():View(x);}
    [HttpPost,ActionName("Delete"),ValidateAntiForgeryToken] public async Task<IActionResult> DeleteConfirmed(int id){var x=await _context.Events.FindAsync(id);if(x is null)return NotFound();_context.Remove(x);await _context.SaveChangesAsync();return RedirectToAction(nameof(Index));}

    private async Task<EventViewModel?> FindModelAsync(int id)=>await _context.Events.Include(x=>x.Alumni).AsNoTracking().Where(x=>x.Id==id).Select(x=>new EventViewModel{Id=x.Id,Title=x.Title,Description=x.Description,EventDate=x.EventDate,AlumniId=x.AlumniId,AlumniName=x.Alumni!.FullName}).FirstOrDefaultAsync();
    private async Task LoadAlumniAsync(int? selectedId=null){var alumni=await _context.Alumni.AsNoTracking().OrderBy(x=>x.FullName).ToListAsync();ViewBag.AlumniId=new SelectList(alumni,"Id","FullName",selectedId);}
}

public sealed class EventViewModel
{
    public int Id{get;set;}
    [Required(ErrorMessage="عنوان الفعالية مطلوب")] public string Title{get;set;}=string.Empty;
    [Required(ErrorMessage="وصف الفعالية مطلوب")] public string Description{get;set;}=string.Empty;
    [Required(ErrorMessage="تاريخ الفعالية مطلوب")] public DateTime EventDate{get;set;}
    [Range(1,int.MaxValue,ErrorMessage="يجب اختيار الخريج")] public int AlumniId{get;set;}
    public string? AlumniName{get;set;}
}
