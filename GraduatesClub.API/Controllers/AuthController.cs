using GraduatesClub.Domain.Security;
using GraduatesClub.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GraduatesClub.API.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly AppDbContext _db; public AuthController(AppDbContext db)=>_db=db;
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request,CancellationToken ct){var email=request.Email.Trim().ToLowerInvariant();var user=await _db.UserProfiles.AsNoTracking().FirstOrDefaultAsync(x=>x.Email==email,ct);return user is not null&&PasswordSecurity.Verify(request.Password,user.PasswordHash)?Ok(new{user.Id,user.FullName,user.Email}):Unauthorized(new ProblemDetails{Title="Invalid credentials",Detail="Email or password is incorrect."});}
}
public sealed class LoginRequest { [Required,EmailAddress] public string Email{get;set;}=string.Empty; [Required] public string Password{get;set;}=string.Empty; }
