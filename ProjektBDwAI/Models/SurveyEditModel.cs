namespace ProjektBDwAI.Models
{
    public class SurveyEditModel
    {
        public int Action { get; set; }
        public Survey Survey { get; set; }
        public string QuestionContent { get; set; }
        public Answer Answer { get; set; }
        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
