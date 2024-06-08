using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]

        public string? UserEmail { get; set; }
        [Required]
        [Display(Name = "Parola")]
        [DataType(DataType.Password)]
        [StringLength(15 , ErrorMessage ="{0} alanı en az {2} karakter olmalıdır" , MinimumLength = 6)]
        public string? Password { get; set; }
    }
}
