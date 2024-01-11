namespace ProjektBDwAI.Models
{
    public class SurveyShowModel
    {
        public Survey Survey { get; set; }
        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
