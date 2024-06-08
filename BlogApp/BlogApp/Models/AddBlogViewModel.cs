using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class AddBlogViewModel
    {
        [Required]
        [Display(Name = "Başlık alanı zorunludur")]
        public string? Title { get; set; }

        
        [Display(Name = "Resim Alanı Zorunludur")]
        public string? Image { get; set; }

        [Required]
        [Display(Name = "Açıklama Alanı zorunludur")]
        public string? Description { get; set; }



    }
}
