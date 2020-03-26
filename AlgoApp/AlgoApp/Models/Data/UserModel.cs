namespace AlgoApp.Models.Data
{
    public class UserModel : CommonResultModel
    {
        public string Username { get; set; }
        public string NickName { get; set; }
        public UserRole Role { get; set; }
    }
}
