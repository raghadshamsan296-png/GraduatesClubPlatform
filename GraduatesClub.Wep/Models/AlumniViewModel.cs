using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GraduatesClub.Web.ViewModels
{
    public class AlumniViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الخريج مطلوب")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "الاسم يجب أن يكون بين 3 و150 حرفاً")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [StringLength(30)]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "سنة التخرج مطلوبة")]
        [Range(1950, 2100, ErrorMessage = "سنة التخرج غير صحيحة")]
        public int GraduationYear { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "يجب اختيار القسم")]
        public int DepartmentId { get; set; }

        [Display(Name = "الصورة الشخصية")]
        public IFormFile? Photo { get; set; }

        public string? ExistingPhotoPath { get; set; }
    }
}
