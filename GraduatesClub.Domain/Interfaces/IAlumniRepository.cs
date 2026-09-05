using GraduatesClub.Domain.Entities;

namespace GraduatesClub.Domain.Interfaces;

public interface IAlumniRepository : IGenericRepository<Alumni>
{
    Task<Alumni?> GetWithDepartmentByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default);
}
