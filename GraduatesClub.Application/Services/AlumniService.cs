using GraduatesClub.Application.Common.Exceptions;
using GraduatesClub.Application.DTOs;
using GraduatesClub.Application.Interfaces;
using GraduatesClub.Domain.Entities;
using GraduatesClub.Domain.Interfaces;

namespace GraduatesClub.Application.Services;

public sealed class AlumniService : IAlumniService
{
    private readonly IAlumniRepository _alumniRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public AlumniService(IAlumniRepository alumniRepository, IDepartmentRepository departmentRepository)
    {
        _alumniRepository = alumniRepository;
        _departmentRepository = departmentRepository;
    }

    public async Task<IReadOnlyList<AlumniDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var alumni = await _alumniRepository.GetAllAsync(cancellationToken);
        return alumni.Select(MapToDto).ToList();
    }

    public async Task<AlumniDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var alumni = await _alumniRepository.GetWithDepartmentByIdAsync(id, cancellationToken);
        return alumni is null ? null : MapToDto(alumni);
    }

    public async Task<AlumniDto> CreateAsync(CreateAlumniDto dto, CancellationToken cancellationToken = default)
    {
        var fullName = NormalizeRequired(dto.FullName, nameof(dto.FullName));
        var email = NormalizeRequired(dto.Email, nameof(dto.Email)).ToLowerInvariant();
        var phone = NormalizeRequired(dto.Phone, nameof(dto.Phone));

        var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId, cancellationToken)
            ?? throw new ArgumentException("The specified DepartmentId does not exist.");

        if (await _alumniRepository.EmailExistsAsync(email, cancellationToken: cancellationToken))
            throw new ConflictException("An alumni record with the same email already exists.");

        var entity = new Alumni
        {
            FullName = fullName,
            Email = email,
            Phone = phone,
            GraduationYear = dto.GraduationYear,
            DepartmentId = dto.DepartmentId
        };

        await _alumniRepository.AddAsync(entity, cancellationToken);
        await _alumniRepository.SaveChangesAsync(cancellationToken);

        return new AlumniDto
        {
            Id = entity.Id,
            FullName = entity.FullName,
            Email = entity.Email,
            Phone = entity.Phone,
            GraduationYear = entity.GraduationYear,
            PhotoPath = entity.PhotoPath,
            DepartmentId = entity.DepartmentId,
            DepartmentName = department.Name
        };
    }

    public async Task<bool> UpdateAsync(UpdateAlumniDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _alumniRepository.GetByIdAsync(dto.Id, cancellationToken);
        if (entity is null)
            return false;

        var fullName = NormalizeRequired(dto.FullName, nameof(dto.FullName));
        var email = NormalizeRequired(dto.Email, nameof(dto.Email)).ToLowerInvariant();
        var phone = NormalizeRequired(dto.Phone, nameof(dto.Phone));

        _ = await _departmentRepository.GetByIdAsync(dto.DepartmentId, cancellationToken)
            ?? throw new ArgumentException("The specified DepartmentId does not exist.");

        if (await _alumniRepository.EmailExistsAsync(email, dto.Id, cancellationToken))
            throw new ConflictException("Another alumni record already uses this email.");

        entity.FullName = fullName;
        entity.Email = email;
        entity.Phone = phone;
        entity.GraduationYear = dto.GraduationYear;
        entity.DepartmentId = dto.DepartmentId;

        _alumniRepository.Update(entity);
        await _alumniRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _alumniRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return false;

        _alumniRepository.Delete(entity);
        await _alumniRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static AlumniDto MapToDto(Alumni entity) => new()
    {
        Id = entity.Id,
        FullName = entity.FullName,
        Email = entity.Email,
        Phone = entity.Phone,
        GraduationYear = entity.GraduationYear,
        PhotoPath = entity.PhotoPath,
        DepartmentId = entity.DepartmentId,
        DepartmentName = entity.Department?.Name
    };

    private static string NormalizeRequired(string value, string fieldName)
    {
        var normalized = value.Trim();
        return normalized.Length == 0
            ? throw new ArgumentException($"{fieldName} cannot be empty or whitespace.")
            : normalized;
    }
}
