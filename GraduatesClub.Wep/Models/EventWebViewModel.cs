using System.ComponentModel.DataAnnotations;

namespace GraduatesClub.Wep.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "عنوان الفعالية مطلوب")]
        [Display(Name = "عنوان الفعالية")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "الوصف")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "تاريخ الفعالية")]
        public DateTime EventDate { get; set; } = DateTime.Now;

        [Display(Name = "المكان")]
        public string Location { get; set; } = string.Empty;
    }
}
