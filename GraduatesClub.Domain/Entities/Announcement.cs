using System.ComponentModel.DataAnnotations;
namespace GraduatesClub.Domain.Entities;

public sealed class Announcement
{
    public int Id { get; set; }
    [Required, StringLength(200)] public string Title { get; set; } = string.Empty;
    [Required, StringLength(4000)] public string Content { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}
