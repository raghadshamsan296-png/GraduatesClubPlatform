using GraduatesClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GraduatesClub.Infrastructure.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Alumni> Alumni => Set<Alumni>();
    public DbSet<ClubEvent> Events => Set<ClubEvent>();
    public DbSet<JobPosting> Jobs => Set<JobPosting>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
