using GraduatesClub.Domain.Entities;
using GraduatesClub.Domain.Interfaces;
using GraduatesClub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraduatesClub.Infrastructure.Repositories;

public sealed class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(AppDbContext context) : base(context)
    {
    }

    public Task<bool> NameExistsAsync(string name, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking().Where(x => x.Name == name);

        if (excludeId.HasValue)
            query = query.Where(x => x.Id != excludeId.Value);

        return query.AnyAsync(cancellationToken);
    }
}
