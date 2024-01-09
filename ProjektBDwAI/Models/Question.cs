using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektBDwAI.Models
{
    public class Question
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SurveyId {  get; set; }
        public required string QuestionType {  get; set; }
        public required string Content { get; set; }

        [ForeignKey(nameof(SurveyId))]
        public required Survey Survey { get; set; }

        public required ICollection<Answer> Answers { get; set; }
    }
}
