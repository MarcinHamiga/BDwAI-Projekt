using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektBDwAI.Models
{
    public class Survey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id {  get; set; }
        [Required]
        public bool IsPublic { get; set; }
        [Required]
        public int OwnerId { get; set; }
        [Required]
        public string Title {  get; set; }

        [ForeignKey(nameof(OwnerId))]
        public User Owner { get; set; }

        public ICollection<Question> Questions { get; set; }
        public ICollection<Result> Results { get; set; }
    }
}
