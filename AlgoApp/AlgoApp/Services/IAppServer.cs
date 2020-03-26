using AlgoApp.Models.Data;
using System.Threading.Tasks;

namespace AlgoApp.Services
{
    interface IAppServer
    {
        string Domain { get; set; }
        Task<LoginResultModel> LoginAsync(string username, string password);
        Task<UserModel> GetCurrentUserAsync();
        Task<LoginResultModel> RegisterAsync(string username, string password);
        Task<ChapterListModel> GetChaperListAsync();
        Task<QuestionListModel> GetQuestionListAsync(int chapterId);
        Task<QuestionModel> GetQuestionAsync(int questionId);
        Task<AnswerResultModel> PostAnswerAsync(int questionId, int answerId);
        void Logout();
        Task<ClassRoomListModel> MyClassRooms();
    }
}
