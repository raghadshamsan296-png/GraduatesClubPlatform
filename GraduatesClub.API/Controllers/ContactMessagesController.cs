using GraduatesClub.Domain.Entities;
using GraduatesClub.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduatesClub.API.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class ContactMessagesController : ControllerBase
{
    private readonly AppDbContext _db; public ContactMessagesController(AppDbContext db)=>_db=db;
    [HttpGet] public async Task<ActionResult<List<ContactMessage>>> GetAll(CancellationToken ct)=>await _db.ContactMessages.AsNoTracking().OrderByDescending(x=>x.SentAt).ToListAsync(ct);
    [HttpGet("{id:int}")] public async Task<ActionResult<ContactMessage>> Get(int id,CancellationToken ct)=>await _db.ContactMessages.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id,ct) is {} x?x:NotFound();
    [HttpPost] public async Task<ActionResult<ContactMessage>> Create(ContactMessage model,CancellationToken ct){model.Id=0;model.SentAt=DateTime.UtcNow;_db.Add(model);await _db.SaveChangesAsync(ct);return CreatedAtAction(nameof(Get),new{id=model.Id},model);}
    [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id,ContactMessage model,CancellationToken ct){var x=await _db.ContactMessages.FindAsync(new object[]{id},ct);if(x is null)return NotFound();x.FullName=model.FullName.Trim();x.Email=model.Email.Trim().ToLowerInvariant();x.Message=model.Message.Trim();await _db.SaveChangesAsync(ct);return NoContent();}
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id,CancellationToken ct){var x=await _db.ContactMessages.FindAsync(new object[]{id},ct);if(x is null)return NotFound();_db.Remove(x);await _db.SaveChangesAsync(ct);return NoContent();}
}
