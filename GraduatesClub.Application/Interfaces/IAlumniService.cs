using GraduatesClub.Application.DTOs;

namespace GraduatesClub.Application.Interfaces;

public interface IAlumniService
{
    Task<IReadOnlyList<AlumniDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AlumniDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<AlumniDto> CreateAsync(CreateAlumniDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(UpdateAlumniDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
