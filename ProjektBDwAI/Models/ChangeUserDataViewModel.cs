using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace ProjektBDwAI.Models
{
    public class ComplexPassword : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string password = value as string;

            if (string.IsNullOrEmpty(password)) return false;

            bool hasLetter = password.Any(char.IsLetter);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecialChar = password.Any(c => !char.IsLetterOrDigit(c));

            return hasLetter && hasDigit && hasSpecialChar;
        }
    }
    public class ChangeUserDataViewModel
    {
        [Required(ErrorMessage = "Hasło jest wymagane")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Hasło jest za krótkie")]
        [ComplexPassword(ErrorMessage = "Hasło musi zawierać co najmniej jedną literę, cyfrę oraz znak specjalny")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Proszę ponownie wpisać nowe hasło")]
        public string RetypePassword { get; set; }

    }
}
