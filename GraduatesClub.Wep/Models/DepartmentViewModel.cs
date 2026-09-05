using System.ComponentModel.DataAnnotations;

namespace GraduatesClub.Web.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم القسم مطلوب")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز اسم القسم 100 حرف")]
        [Display(Name = "اسم القسم")]
        public string Name { get; set; } = string.Empty;
    }
}
