﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektBDwAI.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana")]
        [StringLength(50, MinimumLength = 3, ErrorMessage ="Minimum 3 znaki, maksymalnie 50 znaków")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Minimum 3 znaki, maksymalnie 50 znaków")]
        public string Password { get; set; }
        public bool isAdmin { get; set; }

        public required ICollection<Survey> Surveys { get; set; }
    }
}
