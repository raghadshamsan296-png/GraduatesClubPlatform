namespace GraduatesClub.Application.DTOs;

public sealed class EventDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public int AlumniId { get; set; }
    public string? AlumniName { get; set; }
}
