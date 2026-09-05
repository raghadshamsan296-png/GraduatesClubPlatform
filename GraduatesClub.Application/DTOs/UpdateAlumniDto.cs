using System.ComponentModel.DataAnnotations;

namespace GraduatesClub.Application.DTOs;

public sealed class UpdateAlumniDto
{
    [Range(1, int.MaxValue)]
    public int Id { get; set; }

    [Required, StringLength(150, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(30, MinimumLength = 5)]
    public string Phone { get; set; } = string.Empty;

    [Range(1950, 2100)]
    public int GraduationYear { get; set; }

    [Range(1, int.MaxValue)]
    public int DepartmentId { get; set; }
}
