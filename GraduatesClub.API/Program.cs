using GraduatesClub.Application;
using GraduatesClub.API.Middleware;
using GraduatesClub.Infrastructure;
using GraduatesClub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using GraduatesClub.Domain.Entities;
using GraduatesClub.Domain.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options => options.AddPolicy("MobileApp", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Graduates Club API",
        Version = "v1",
        Description = "API for departments, alumni, and graduates club events."
    });
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

// LocalDB is created automatically on first run for easy Swagger testing.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
    if (!await dbContext.UserProfiles.AnyAsync())
    {
        dbContext.UserProfiles.Add(new UserProfile { FullName = "مدير النظام", Email = "admin@graduates.local", Phone = string.Empty, PasswordHash = PasswordSecurity.Hash("Admin@123") });
        await dbContext.SaveChangesAsync();
    }
}

// Keep local HTTP available for the Android emulator/physical phone.
// Production environments are still redirected to HTTPS.
if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();
app.UseCors("MobileApp");
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
