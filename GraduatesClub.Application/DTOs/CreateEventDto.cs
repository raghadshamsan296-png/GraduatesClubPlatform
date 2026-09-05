using System.ComponentModel.DataAnnotations;

namespace GraduatesClub.Application.DTOs;

public sealed class CreateEventDto
{
    [Required, StringLength(200, MinimumLength = 2)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(2000, MinimumLength = 2)]
    public string Description { get; set; } = string.Empty;

    public DateTime EventDate { get; set; }

    [Range(1, int.MaxValue)]
    public int AlumniId { get; set; }
}
