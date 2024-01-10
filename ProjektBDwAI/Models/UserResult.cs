using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjektBDwAI.Models
{
    public class UserResult
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        [Required]
        public int ResultId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public string SelectedAnswer { get; set; }

        [ForeignKey(nameof(QuestionId))]
        public Question Question { get; set; }

        [ForeignKey(nameof(ResultId))]
        public Result Result { get; set; }
    }
}
