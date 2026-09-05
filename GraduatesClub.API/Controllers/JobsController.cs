using GraduatesClub.Domain.Entities;
using GraduatesClub.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduatesClub.API.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class JobsController : ControllerBase
{
    private readonly AppDbContext _db;
    public JobsController(AppDbContext db) => _db = db;
    [HttpGet] public async Task<ActionResult<List<JobPosting>>> GetAll(CancellationToken ct) => await _db.Jobs.AsNoTracking().OrderByDescending(x => x.CreatedAt).ToListAsync(ct);
    [HttpGet("{id:int}")] public async Task<ActionResult<JobPosting>> Get(int id, CancellationToken ct) => await _db.Jobs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct) is { } x ? x : NotFound();
    [HttpPost] public async Task<ActionResult<JobPosting>> Create(JobPosting model, CancellationToken ct) { model.Id = 0; model.CreatedAt = DateTime.UtcNow; _db.Jobs.Add(model); await _db.SaveChangesAsync(ct); return CreatedAtAction(nameof(Get), new { id = model.Id }, model); }
    [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id, JobPosting model, CancellationToken ct) { var x = await _db.Jobs.FindAsync(new object[] { id }, ct); if (x is null) return NotFound(); x.JobTitle=model.JobTitle; x.CompanyName=model.CompanyName; x.Requirements=model.Requirements; await _db.SaveChangesAsync(ct); return NoContent(); }
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id, CancellationToken ct) { var x=await _db.Jobs.FindAsync(new object[] { id }, ct); if(x is null)return NotFound(); _db.Jobs.Remove(x); await _db.SaveChangesAsync(ct); return NoContent(); }
}
