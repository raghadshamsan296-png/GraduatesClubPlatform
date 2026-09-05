using System.ComponentModel.DataAnnotations;

namespace GraduatesClub.Web.Models;

public sealed class ContactViewModel
{
    [Required(ErrorMessage = "الاسم مطلوب")]
    [StringLength(150, MinimumLength = 3)]
    public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "نص الرسالة مطلوب")]
    [StringLength(4000, MinimumLength = 5)]
    public string Message { get; set; } = string.Empty;
}
