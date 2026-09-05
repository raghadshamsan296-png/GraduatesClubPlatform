using System.ComponentModel.DataAnnotations;
namespace GraduatesClub.Domain.Entities;

public sealed class JobPosting
{
    public int Id { get; set; }
    [Required, StringLength(200)] public string JobTitle { get; set; } = string.Empty;
    [Required, StringLength(200)] public string CompanyName { get; set; } = string.Empty;
    [Required, StringLength(2000)] public string Requirements { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
