using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Login jest wymagany.")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamiętać?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int? Id { get; set; }

        [Required(ErrorMessage = "Login jest wymagany.")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Numer legitymacji jest wymagany.")]
        [Display(Name = "Numer legitymacji")]
        public string StudentCardId { get; set; }

        [Required(ErrorMessage = "Średnia jest wymagana.")]
        [Display(Name = "Średnia")]
        public string AverageScore { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(100, ErrorMessage = "{0} musi mieć przynajmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Podane hasła nie pasują do siebie.")]
        public string ConfirmPassword { get; set; }
    }
}
