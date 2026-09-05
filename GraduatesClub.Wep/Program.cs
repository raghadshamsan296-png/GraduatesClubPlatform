using Microsoft.EntityFrameworkCore;
using GraduatesClub.Infrastructure.Data; // تأكد أن الـ namespace يتوافق مع مشروعك
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GraduatesClub.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => { options.LoginPath = "/Login/Index"; options.ExpireTimeSpan = TimeSpan.FromHours(8); });
            builder.Services.AddAuthorization(options => options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

            // === أضف هذا السطر لتسجيل قاعدة البيانات وحل المشكلة جذرياً ===
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
                if (!dbContext.UserProfiles.Any())
                {
                    dbContext.UserProfiles.Add(new GraduatesClub.Domain.Entities.UserProfile
                    {
                        FullName = "مدير النظام", Email = "admin@graduates.local", Phone = string.Empty,
                        PasswordHash = GraduatesClub.Domain.Security.PasswordSecurity.Hash("Admin@123")
                    });
                    dbContext.SaveChanges();
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
