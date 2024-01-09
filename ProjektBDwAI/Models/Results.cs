using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektBDwAI.Models
{
    public class Results
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public int UserId { get; set; }
        public DateTime DateFilled { get; set; }

        [ForeignKey(nameof(SurveyId))]
        public Survey Survey { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public ICollection<UserResults> UserResults { get; set; }
    }
}
