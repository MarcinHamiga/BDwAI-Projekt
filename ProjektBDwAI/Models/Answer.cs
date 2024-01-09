using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektBDwAI.Models
{
    public class Answer
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public required string Content { get; set; }
        [Required]
        public required string Value {get; set; }
        [Required]
        public required string Name { get; set; }

        [ForeignKey(nameof(QuestionId))]
        public required Question Question { get; set; }
    }
}
