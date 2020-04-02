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
        Task<QuestionModel> GetQuestionWithAnswerAsync(int questionId, int answerId);
        Task<AnswerResultModel> PostAnswerAsync(int questionId, int answerId);
        void Logout();
        Task<ClassRoomListModel> MyClassRooms();
        Task<ClassRoomModel> ClassRoom(int id);
        Task<ClassRoomModel> AddClassRoom(string name);
        Task<UserModel> GetUserDetail(int id);
        Task<CommonResultModel> DeleteClassRomm(int id);
        Task<CommonResultModel> RenameClassRomm(int id, string newName);
        Task<UserListModel> SearchStudentsNotInClass(int excludeClassId, string name);
        Task<CommonResultModel> AddStudentToClass(int studentId, int classId);
        Task<CommonResultModel> RemoveStudentFromClass(int studentId, int classId);
        Task<HistoryListModel> GetUserAnswerHistory(int studentId);
    }
}
