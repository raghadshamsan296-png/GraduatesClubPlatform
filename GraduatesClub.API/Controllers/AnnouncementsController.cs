using GraduatesClub.Domain.Entities;
using GraduatesClub.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduatesClub.API.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class AnnouncementsController : ControllerBase
{
    private readonly AppDbContext _db;
    public AnnouncementsController(AppDbContext db) => _db=db;
    [HttpGet] public async Task<ActionResult<List<Announcement>>> GetAll(CancellationToken ct)=>await _db.Announcements.AsNoTracking().OrderByDescending(x=>x.Date).ToListAsync(ct);
    [HttpGet("{id:int}")] public async Task<ActionResult<Announcement>> Get(int id,CancellationToken ct)=>await _db.Announcements.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id,ct) is {} x?x:NotFound();
    [HttpPost] public async Task<ActionResult<Announcement>> Create(Announcement model,CancellationToken ct){model.Id=0;_db.Add(model);await _db.SaveChangesAsync(ct);return CreatedAtAction(nameof(Get),new{id=model.Id},model);}
    [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id,Announcement model,CancellationToken ct){var x=await _db.Announcements.FindAsync(new object[]{id},ct);if(x is null)return NotFound();x.Title=model.Title;x.Content=model.Content;x.Date=model.Date;await _db.SaveChangesAsync(ct);return NoContent();}
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id,CancellationToken ct){var x=await _db.Announcements.FindAsync(new object[]{id},ct);if(x is null)return NotFound();_db.Remove(x);await _db.SaveChangesAsync(ct);return NoContent();}
}
