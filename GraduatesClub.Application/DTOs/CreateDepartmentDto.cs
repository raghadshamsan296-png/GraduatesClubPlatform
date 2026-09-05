using System.ComponentModel.DataAnnotations;

namespace GraduatesClub.Application.DTOs;

public sealed class CreateDepartmentDto
{
    [Required, StringLength(120, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
}
