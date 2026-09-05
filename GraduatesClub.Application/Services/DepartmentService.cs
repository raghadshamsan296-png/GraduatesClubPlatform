using GraduatesClub.Application.Common.Exceptions;
using GraduatesClub.Application.DTOs;
using GraduatesClub.Application.Interfaces;
using GraduatesClub.Domain.Entities;
using GraduatesClub.Domain.Interfaces;

namespace GraduatesClub.Application.Services;

public sealed class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<IReadOnlyList<DepartmentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var departments = await _departmentRepository.GetAllAsync(cancellationToken);
        return departments.Select(MapToDto).ToList();
    }

    public async Task<DepartmentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var department = await _departmentRepository.GetByIdAsync(id, cancellationToken);
        return department is null ? null : MapToDto(department);
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto, CancellationToken cancellationToken = default)
    {
        var name = NormalizeRequired(dto.Name);

        if (await _departmentRepository.NameExistsAsync(name, cancellationToken: cancellationToken))
            throw new ConflictException("A department with the same name already exists.");

        var entity = new Department { Name = name };
        await _departmentRepository.AddAsync(entity, cancellationToken);
        await _departmentRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(entity);
    }

    public async Task<bool> UpdateAsync(UpdateDepartmentDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _departmentRepository.GetByIdAsync(dto.Id, cancellationToken);
        if (entity is null)
            return false;

        var name = NormalizeRequired(dto.Name);

        if (await _departmentRepository.NameExistsAsync(name, dto.Id, cancellationToken))
            throw new ConflictException("Another department already uses this name.");

        entity.Name = name;
        _departmentRepository.Update(entity);
        await _departmentRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _departmentRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return false;

        _departmentRepository.Delete(entity);
        await _departmentRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static DepartmentDto MapToDto(Department entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name
    };

    private static string NormalizeRequired(string value)
    {
        var normalized = value.Trim();
        return normalized.Length == 0
            ? throw new ArgumentException("Department name cannot be empty or whitespace.")
            : normalized;
    }
}
