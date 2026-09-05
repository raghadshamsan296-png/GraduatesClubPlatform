using System.ComponentModel.DataAnnotations;

namespace GraduatesClub.Web.Models;

public sealed class UserProfileViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "الاسم مطلوب"), StringLength(150, MinimumLength = 3)]
    public string FullName { get; set; } = string.Empty;
    [Required(ErrorMessage = "البريد مطلوب"), EmailAddress(ErrorMessage = "صيغة البريد غير صحيحة"), StringLength(200)]
    public string Email { get; set; } = string.Empty;
    [Phone(ErrorMessage = "رقم الهاتف غير صحيح"), StringLength(30)]
    public string? Phone { get; set; }
    [MinLength(8, ErrorMessage = "كلمة المرور يجب ألا تقل عن 8 أحرف")]
    public string? Password { get; set; }
}
