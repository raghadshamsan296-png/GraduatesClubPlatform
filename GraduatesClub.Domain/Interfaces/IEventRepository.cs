using GraduatesClub.Domain.Entities;

namespace GraduatesClub.Domain.Interfaces;

public interface IEventRepository : IGenericRepository<ClubEvent>
{
    Task<ClubEvent?> GetWithAlumniByIdAsync(int id, CancellationToken cancellationToken = default);
}
