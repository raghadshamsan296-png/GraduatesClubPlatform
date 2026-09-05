using GraduatesClub.Application.DTOs;
using GraduatesClub.Application.Interfaces;
using GraduatesClub.Domain.Entities;
using GraduatesClub.Domain.Interfaces;

namespace GraduatesClub.Application.Services;

public sealed class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IAlumniRepository _alumniRepository;

    public EventService(IEventRepository eventRepository, IAlumniRepository alumniRepository)
    {
        _eventRepository = eventRepository;
        _alumniRepository = alumniRepository;
    }

    public async Task<IReadOnlyList<EventDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var events = await _eventRepository.GetAllAsync(cancellationToken);
        return events.Select(MapToDto).ToList();
    }

    public async Task<EventDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var clubEvent = await _eventRepository.GetWithAlumniByIdAsync(id, cancellationToken);
        return clubEvent is null ? null : MapToDto(clubEvent);
    }

    public async Task<EventDto> CreateAsync(CreateEventDto dto, CancellationToken cancellationToken = default)
    {
        var title = NormalizeRequired(dto.Title, nameof(dto.Title));
        var description = NormalizeRequired(dto.Description, nameof(dto.Description));
        ValidateEventDate(dto.EventDate);

        var alumni = await _alumniRepository.GetByIdAsync(dto.AlumniId, cancellationToken)
            ?? throw new ArgumentException("The specified AlumniId does not exist.");

        var entity = new ClubEvent
        {
            Title = title,
            Description = description,
            EventDate = dto.EventDate,
            AlumniId = dto.AlumniId
        };

        await _eventRepository.AddAsync(entity, cancellationToken);
        await _eventRepository.SaveChangesAsync(cancellationToken);

        return new EventDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            EventDate = entity.EventDate,
            AlumniId = entity.AlumniId,
            AlumniName = alumni.FullName
        };
    }

    public async Task<bool> UpdateAsync(UpdateEventDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _eventRepository.GetByIdAsync(dto.Id, cancellationToken);
        if (entity is null)
            return false;

        var title = NormalizeRequired(dto.Title, nameof(dto.Title));
        var description = NormalizeRequired(dto.Description, nameof(dto.Description));
        ValidateEventDate(dto.EventDate);

        _ = await _alumniRepository.GetByIdAsync(dto.AlumniId, cancellationToken)
            ?? throw new ArgumentException("The specified AlumniId does not exist.");

        entity.Title = title;
        entity.Description = description;
        entity.EventDate = dto.EventDate;
        entity.AlumniId = dto.AlumniId;

        _eventRepository.Update(entity);
        await _eventRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _eventRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return false;

        _eventRepository.Delete(entity);
        await _eventRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static EventDto MapToDto(ClubEvent entity) => new()
    {
        Id = entity.Id,
        Title = entity.Title,
        Description = entity.Description,
        EventDate = entity.EventDate,
        AlumniId = entity.AlumniId,
        AlumniName = entity.Alumni?.FullName
    };

    private static string NormalizeRequired(string value, string fieldName)
    {
        var normalized = value.Trim();
        return normalized.Length == 0
            ? throw new ArgumentException($"{fieldName} cannot be empty or whitespace.")
            : normalized;
    }

    private static void ValidateEventDate(DateTime eventDate)
    {
        if (eventDate == default)
            throw new ArgumentException("EventDate must contain a valid date and time.");
    }
}
