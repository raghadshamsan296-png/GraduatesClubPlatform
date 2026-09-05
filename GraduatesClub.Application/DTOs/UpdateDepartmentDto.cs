using System.ComponentModel.DataAnnotations;

namespace GraduatesClub.Application.DTOs;

public sealed class UpdateDepartmentDto
{
    [Range(1, int.MaxValue)]
    public int Id { get; set; }

    [Required, StringLength(120, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
}
