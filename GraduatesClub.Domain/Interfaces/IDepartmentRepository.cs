using GraduatesClub.Domain.Entities;

namespace GraduatesClub.Domain.Interfaces;

public interface IDepartmentRepository : IGenericRepository<Department>
{
    Task<bool> NameExistsAsync(string name, int? excludeId = null, CancellationToken cancellationToken = default);
}
