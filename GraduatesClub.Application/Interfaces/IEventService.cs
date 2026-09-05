using GraduatesClub.Application.DTOs;

namespace GraduatesClub.Application.Interfaces;

public interface IEventService
{
    Task<IReadOnlyList<EventDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<EventDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<EventDto> CreateAsync(CreateEventDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(UpdateEventDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
