namespace AlgoApp.Models.Data
{
    public class EasyToGetWrongQuestionModel
    {
        public int QuestionId { get; set; }
        public string Content { get; set; }
        public int ErrorCount { get; set; }
        public string UserNickname { get; set; }
        public string UserAnswer { get; set; }
    }
}
