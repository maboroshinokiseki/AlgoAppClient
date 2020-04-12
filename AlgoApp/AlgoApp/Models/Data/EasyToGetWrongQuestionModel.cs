namespace AlgoApp.Models.Data
{
    public class EasyToGetWrongQuestionModel
    {
        public int ChapterId { get; set; }
        public int QuestionId { get; set; }
        public string Content { get; set; }
        public double ErrorRatio { get; set; }
        public string UserNickname { get; set; }
        public string UserAnswer { get; set; }
    }
}
