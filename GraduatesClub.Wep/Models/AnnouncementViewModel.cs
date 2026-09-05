namespace GraduatesClub.Web.Models
{
    public class AnnouncementViewModel
    {
        public int Id { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "عنوان الإعلان مطلوب")]
        [System.ComponentModel.DataAnnotations.StringLength(200, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "محتوى الإعلان مطلوب")]
        [System.ComponentModel.DataAnnotations.StringLength(4000, MinimumLength = 5)]
        public string Content { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
