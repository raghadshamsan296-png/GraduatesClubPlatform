namespace GraduatesClub.Application.DTOs;

public sealed class AlumniDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int GraduationYear { get; set; }
    public string? PhotoPath { get; set; }
    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
}
