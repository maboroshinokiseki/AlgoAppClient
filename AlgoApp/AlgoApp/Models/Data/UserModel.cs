using System;

namespace AlgoApp.Models.Data
{
    public class UserModel : CommonResultModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string NickName { get; set; }
        public int Points { get; set; }
        public UserRole Role { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDay { get; set; }
        public double CorrectRatio { get; set; }
        public double DoneQuestionCount { get; set; }
    }
}
