using System.ComponentModel.DataAnnotations;
namespace GraduatesClub.Domain.Entities;

public sealed class ContactMessage
{
    public int Id { get; set; }
    [Required, StringLength(150)] public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress, StringLength(200)] public string Email { get; set; } = string.Empty;
    [Required, StringLength(4000)] public string Message { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
