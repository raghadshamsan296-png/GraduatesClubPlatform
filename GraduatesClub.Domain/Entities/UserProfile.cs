using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GraduatesClub.Domain.Entities;

public sealed class UserProfile
{
    public int Id { get; set; }
    [Required, StringLength(150, MinimumLength = 3)]
    public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;
    [Phone, StringLength(30)]
    public string Phone { get; set; } = string.Empty;
    [JsonIgnore]
    public string? PasswordHash { get; set; }
}
