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
        public string QuestionType {  get; set; }
        public string Content { get; set; }

        [ForeignKey(nameof(SurveyId))]
        public Survey Survey { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}
