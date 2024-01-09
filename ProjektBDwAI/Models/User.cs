using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektBDwAI.Models
{
    public class User : IdentityUser<int>
    {

        [Key]
        public override int Id { get; set; }

        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana")]
        [StringLength(50, MinimumLength = 3, ErrorMessage ="Minimum 3 znaki, maksymalnie 50 znaków")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Minimum 3 znaki, maksymalnie 50 znaków")]
        public required string Password { get; set; }
        public bool isAdmin { get; set; }

        public ICollection<Survey>? Surveys { get; set; }
    }
}
