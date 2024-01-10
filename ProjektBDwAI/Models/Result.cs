using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektBDwAI.Models
{
    public class Result
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        public int SurveyId { get; set; }
        [Required]
        public DateTime DateFilled { get; set; }

        [ForeignKey(nameof(SurveyId))]
        public Survey Survey { get; set; }

        public ICollection<UserResult> UserResults { get; set; }
    }
}
