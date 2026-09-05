using System.ComponentModel.DataAnnotations;

namespace GraduatesClub.Domain.Entities;

public sealed class Alumni
{
    public int Id { get; set; }
    [Required, StringLength(150, MinimumLength = 3)]
    public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;
    [Phone, StringLength(30)]
    public string Phone { get; set; } = string.Empty;
    [Range(1950, 2100)]
    public int GraduationYear { get; set; }
    public string? PhotoPath { get; set; }

    [Range(1, int.MaxValue)]
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }

    public ICollection<ClubEvent> Events { get; set; } = new List<ClubEvent>();
}
