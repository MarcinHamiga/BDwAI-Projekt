namespace ProjektBDwAI.Models
{
    public class SurveyEditModel
    {
        public int Action { get; set; }
        public string answerContent { get; set; }
        public int questionId { get; set; } 
        public int answerId { get; set; }
        public Survey Survey { get; set; }
        public int surveyId { get; set; }
        public string questionContent { get; set; }
        public string questionType {  get; set; }
        public Answer Answer { get; set; }
        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
