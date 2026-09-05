using System.ComponentModel.DataAnnotations;

namespace GraduatesClub.Domain.Entities;

public sealed class ClubEvent
{
    public int Id { get; set; }
    [Required, StringLength(200, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;
    [Required, StringLength(2000)]
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }

    [Range(1, int.MaxValue)]
    public int AlumniId { get; set; }
    public Alumni? Alumni { get; set; }
}
