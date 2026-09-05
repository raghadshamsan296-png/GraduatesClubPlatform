using System.ComponentModel.DataAnnotations;

namespace GraduatesClub.Domain.Entities;

public sealed class Department
{
    public int Id { get; set; }
    [Required(ErrorMessage = "اسم القسم مطلوب")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "اسم القسم يجب أن يكون بين حرفين و100 حرف")]
    public string Name { get; set; } = string.Empty;

    public ICollection<Alumni> Alumni { get; set; } = new List<Alumni>();
}
