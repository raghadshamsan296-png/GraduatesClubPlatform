namespace GraduatesClub.Web.Models
{
    public class JobViewModel
    {
        public int Id { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "المسمى الوظيفي مطلوب")]
        [System.ComponentModel.DataAnnotations.StringLength(200, MinimumLength = 3)]
        public string JobTitle { get; set; } = string.Empty;
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "اسم الشركة مطلوب")]
        [System.ComponentModel.DataAnnotations.StringLength(200, MinimumLength = 2)]
        public string CompanyName { get; set; } = string.Empty;
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "متطلبات الوظيفة مطلوبة")]
        [System.ComponentModel.DataAnnotations.StringLength(2000)]
        public string Requirements { get; set; } = string.Empty;
    }
}
