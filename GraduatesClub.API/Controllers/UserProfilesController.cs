using GraduatesClub.Domain.Entities;
using GraduatesClub.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GraduatesClub.Domain.Security;
using System.ComponentModel.DataAnnotations;

namespace GraduatesClub.API.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class UserProfilesController : ControllerBase
{
    private readonly AppDbContext _db; public UserProfilesController(AppDbContext db)=>_db=db;
    [HttpGet] public async Task<ActionResult<List<UserProfile>>> GetAll(CancellationToken ct)=>await _db.UserProfiles.AsNoTracking().Select(x=>new UserProfile{Id=x.Id,FullName=x.FullName,Email=x.Email,Phone=x.Phone}).ToListAsync(ct);
    [HttpGet("{id:int}")] public async Task<ActionResult<UserProfile>> Get(int id,CancellationToken ct)=>await _db.UserProfiles.AsNoTracking().Where(x=>x.Id==id).Select(x=>new UserProfile{Id=x.Id,FullName=x.FullName,Email=x.Email,Phone=x.Phone}).FirstOrDefaultAsync(ct) is {} x?x:NotFound();
    [HttpPost] public async Task<ActionResult<UserProfile>> Create(CreateUserProfileRequest request,CancellationToken ct){var model=new UserProfile{FullName=request.FullName.Trim(),Email=request.Email.Trim().ToLowerInvariant(),Phone=request.Phone?.Trim()??string.Empty,PasswordHash=PasswordSecurity.Hash(request.Password)};_db.Add(model);await _db.SaveChangesAsync(ct);return CreatedAtAction(nameof(Get),new{id=model.Id},new UserProfile{Id=model.Id,FullName=model.FullName,Email=model.Email,Phone=model.Phone});}
    [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id,UserProfile model,CancellationToken ct){var x=await _db.UserProfiles.FindAsync(new object[]{id},ct);if(x is null)return NotFound();x.FullName=model.FullName;x.Email=model.Email;x.Phone=model.Phone;await _db.SaveChangesAsync(ct);return NoContent();}
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id,CancellationToken ct){var x=await _db.UserProfiles.FindAsync(new object[]{id},ct);if(x is null)return NotFound();_db.Remove(x);await _db.SaveChangesAsync(ct);return NoContent();}
}
public sealed class CreateUserProfileRequest { [Required] public string FullName{get;set;}=string.Empty; [Required,EmailAddress] public string Email{get;set;}=string.Empty; public string? Phone{get;set;} [Required,MinLength(8)] public string Password{get;set;}=string.Empty; }
