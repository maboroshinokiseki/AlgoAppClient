using AlgoApp.Models.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlgoApp.Services
{
    interface IAppServer
    {
        string Domain { get; set; }
        Task<LoginResultModel> LoginAsync(string username, string password);
        Task<UserModel> GetCurrentUserAsync();
        Task<LoginResultModel> RegisterAsync(string username, string password);
        Task<CommonResultModel> UpdateUserInfo(UserModel model);
        Task<CommonListResultModel<ChapterModel>> GetChaperListAsync();
        Task<CommonListResultModel<QuestionModel>> GetQuestionListAsync(int chapterId);
        Task<QuestionModel> GetQuestionAsync(int questionId);
        Task<QuestionModel> GetQuestionWithAnswerAsync(int questionId, int answerId);
        Task<AnswerResultModel> PostAnswerAsync(int questionId, List<int> answerIds, bool isDailyPractice);
        void Logout();
        Task<CommonListResultModel<ClassRoomModel>> MyClassRooms();
        Task<ClassRoomModel> ClassRoom(int id);
        Task<ClassRoomModel> AddClassRoom(string name);
        Task<UserModel> GetUserDetail(int id);
        Task<CommonResultModel> DeleteClassRomm(int id);
        Task<CommonResultModel> RenameClassRomm(int id, string newName);
        Task<CommonListResultModel<UserModel>> SearchStudentsNotInClass(int excludeClassId, string name);
        Task<CommonListResultModel<ClassRoomModel>> SearchClassImNotIn(string searchText);
        Task<CommonResultModel> JoinClassRomm(int classId);
        Task<CommonResultModel> AddStudentToClass(int studentId, int classId);
        Task<CommonResultModel> RemoveStudentFromClass(int studentId, int classId);
        Task<CommonListResultModel<HistoryItemModel>> GetUserAnswerHistory(int studentId, int chapterId);
        Task<CommonListResultModel<ChapterModel>> GetUserAnswerHistoryChapters(int studentId);
        Task<CommonListResultModel<EasyToGetWrongQuestionModel>> GetEasyToGetWrongChaptersByClass(int classId);
        Task<CommonListResultModel<EasyToGetWrongQuestionModel>> GetEasyToGetWrongQuestionsByClassChapter(int classId, int chapterId);
        Task<CommonListResultModel<EasyToGetWrongQuestionModel>> GetEasyToGetWrongQuestionDetail(int classId,int questionId);
        Task<CommonResultModel> IsQuestionInBookmark(int questionId);
        Task<CommonResultModel> AddQuestionToBookmark(int questionId);
        Task<CommonResultModel> RemoveQuestionFromBookmark(int questionId);
        Task<CommonListResultModel<QuestionModel>> QuestionsInBookmark();
        Task<QuestionModel> GetDailyPracticeQuestion();
        Task<QuestionModel> GetBreakThroughQuestion();
        Task<CommonResultModel> PostMessage(MessageModel message);
        Task<CommonListResultModel<UserModel>> YesterdayTop10();
        Task<CommonListResultModel<UserModel>> AllTimeTop10();
    }
}
