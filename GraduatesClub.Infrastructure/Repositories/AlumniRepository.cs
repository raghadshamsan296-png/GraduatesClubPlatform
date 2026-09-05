using GraduatesClub.Domain.Entities;
using GraduatesClub.Domain.Interfaces;
using GraduatesClub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraduatesClub.Infrastructure.Repositories;

public sealed class AlumniRepository : GenericRepository<Alumni>, IAlumniRepository
{
    public AlumniRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<IReadOnlyList<Alumni>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await DbSet
            .AsNoTracking()
            .Include(x => x.Department)
            .OrderBy(x => x.FullName)
            .ToListAsync(cancellationToken);

    public Task<Alumni?> GetWithDepartmentByIdAsync(int id, CancellationToken cancellationToken = default) =>
        DbSet
            .AsNoTracking()
            .Include(x => x.Department)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<bool> EmailExistsAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking().Where(x => x.Email == email);

        if (excludeId.HasValue)
            query = query.Where(x => x.Id != excludeId.Value);

        return query.AnyAsync(cancellationToken);
    }
}
