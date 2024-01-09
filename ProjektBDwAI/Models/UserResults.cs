namespace ProjektBDwAI.Models
{
    public class UserResults
    {
        public int Id { get; set; }
        public int ResultId { get; set; }
        public int QuestionId { get; set; }
        public required string AnswerText { get; set; }
        public required string SelectedAnswer { get; set; }
    }
}
