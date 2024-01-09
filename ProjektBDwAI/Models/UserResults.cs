using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjektBDwAI.Models
{
    public class UserResults
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int ResultId { get; set; }
        public int QuestionId { get; set; }
        public required string AnswerText { get; set; }
        public required string SelectedAnswer { get; set; }

        [ForeignKey(nameof(QuestionId))]
        public Question Questions { get; set; }

        [ForeignKey(nameof(ResultId))]
        public required Results Results { get; set; }
    }
}
