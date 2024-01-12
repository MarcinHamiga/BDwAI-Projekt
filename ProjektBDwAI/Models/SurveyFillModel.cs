namespace ProjektBDwAI.Models
{
    public class SurveyFillModel
    {
        public Survey Survey { get; set; }
        public int surveyId { get; set; }
        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }
        public List<string> Results { get; set; }

    }
}
