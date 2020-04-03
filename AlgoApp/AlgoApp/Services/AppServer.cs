using AlgoApp.Models.Data;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AlgoApp.Services
{
    class AppServer : IAppServer
    {
        private string token;
        private readonly HttpClient httpClient;
        public AppServer()
        {
            domain = Preferences.Get("Domain", "10.0.2.2:5000");

            LoadToken();
            var handler = new HttpClientHandler { AllowAutoRedirect = true };
            httpClient = new HttpClient(handler)
            {
                BaseAddress = this.BaseAddress,
                Timeout = new TimeSpan(0, 0, 100),
            };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<LoginResultModel> LoginAsync(string username, string password)
        {
            var json = JsonConvert.SerializeObject(new { username, password });
            var result = await QueryAsync<LoginResultModel>(HttpMethod.Post, "user/login", json);
            token = result.Token;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            SaveToken();

            return result;
        }

        public async Task<LoginResultModel> RegisterAsync(string username, string password)
        {
            var json = JsonConvert.SerializeObject(new { username, password });
            var result = await QueryAsync<LoginResultModel>(HttpMethod.Post, "user/register", json);
            token = result.Token;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            SaveToken();

            return result;
        }

        public async Task<CommonListResultModel<ChapterModel>> GetChaperListAsync()
        {
            return await QueryAsync<CommonListResultModel<ChapterModel>>(HttpMethod.Get, "Chapters");
        }

        public async Task<CommonListResultModel<QuestionModel>> GetQuestionListAsync(int chapterId)
        {
            var model = await QueryAsync<CommonListResultModel<QuestionModel>>(HttpMethod.Get, $"Chapters/{chapterId}/questions");
            for (int i = 0; i < model.Items.Count; i++)
            {
                model.Items[i].ContentWithIndex = $"{i + 1}、{model.Items[i].Content}";
            }

            return model;
        }

        public async Task<QuestionModel> GetQuestionAsync(int questionId)
        {
            return await QueryAsync<QuestionModel>(HttpMethod.Get, $"questions/{questionId}");
        }

        private string domain;
        public string Domain
        {
            get => domain;
            set
            {
                Preferences.Set("Domain", value);
                domain = value;
            }
        }

        private Uri BaseAddress => new Uri($"http://{Domain}/api/");

        private void SaveToken()
        {
            Preferences.Set("Token", token);
        }

        private void LoadToken()
        {
            token = Preferences.Get("Token", "");
        }

        private async Task<T> QueryAsync<T>(HttpMethod method, string path, string jsonContent = null) where T : CommonResultModel, new()
        {
            var requestMessage = new HttpRequestMessage(method, path);
            if (!string.IsNullOrEmpty(jsonContent))
            {
                requestMessage.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            }

            string responseContent;
            try
            {
                var responseMessage = await httpClient.SendAsync(requestMessage);
                responseContent = await responseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                return new T() { Code = Codes.TimeOut };
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(responseContent) ?? new T() { Code = Codes.Unknown };
            }
            catch (Exception)
            {
                return new T() { Code = Codes.Unknown };
            }

        }

        public async Task<AnswerResultModel> PostAnswerAsync(int questionId, int answerId)
        {
            var json = JsonConvert.SerializeObject(new { questionId, answerId });
            var result = await QueryAsync<AnswerResultModel>(HttpMethod.Post, "Answer", json);

            return result;
        }

        public async Task<UserModel> GetCurrentUserAsync()
        {
            return await QueryAsync<UserModel>(HttpMethod.Get, "User/CurrentUser");
        }

        public void Logout()
        {
            token = "";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            SaveToken();
        }

        public async Task<CommonListResultModel<ClassRoomModel>> MyClassRooms()
        {
            return await QueryAsync<CommonListResultModel<ClassRoomModel>>(HttpMethod.Get, "ClassRoom/MyClassRooms");
        }

        public async Task<ClassRoomModel> ClassRoom(int id)
        {
            return await QueryAsync<ClassRoomModel>(HttpMethod.Get, $"ClassRoom/ClassRoom/{id}");
        }

        public async Task<UserModel> GetUserDetail(int id)
        {
            return await QueryAsync<UserModel>(HttpMethod.Get, $"User/UserDetail/{id}");
        }

        public async Task<ClassRoomModel> AddClassRoom(string name)
        {
            var json = JsonConvert.SerializeObject(new { name });
            return await QueryAsync<ClassRoomModel>(HttpMethod.Post, $"ClassRoom/AddClassRoom", json);
        }

        public async Task<CommonResultModel> DeleteClassRomm(int id)
        {
            return await QueryAsync<CommonResultModel>(HttpMethod.Delete, $"ClassRoom/DeleteClassRoom/{id}");
        }

        public async Task<CommonResultModel> RenameClassRomm(int id, string newName)
        {
            var json = JsonConvert.SerializeObject(new { newName });
            return await QueryAsync<CommonResultModel>(HttpMethod.Put, $"ClassRoom/RenameClassRomm/{id}", json);
        }

        public async Task<CommonListResultModel<UserModel>> SearchStudentsNotInClass(int excludeClassId, string name)
        {
            return await QueryAsync<CommonListResultModel<UserModel>>(HttpMethod.Get, $"User/SearchStudentsNotInClass/{excludeClassId}/{name}");
        }

        public async Task<CommonResultModel> AddStudentToClass(int studentId, int classId)
        {
            var json = JsonConvert.SerializeObject(new { studentId, classId });
            return await QueryAsync<CommonResultModel>(HttpMethod.Post, $"ClassRoom/AddStudentToClass", json);
        }

        public async Task<CommonResultModel> RemoveStudentFromClass(int studentId, int classId)
        {
            var json = JsonConvert.SerializeObject(new { studentId, classId });
            return await QueryAsync<CommonResultModel>(HttpMethod.Post, $"ClassRoom/RemoveStudentFromClass", json);
        }

        public async Task<CommonListResultModel<HistoryItemModel>> GetUserAnswerHistory(int studentId)
        {
            return await QueryAsync<CommonListResultModel<HistoryItemModel>>(HttpMethod.Get, $"Answer/histories/{studentId}");
        }

        public async Task<QuestionModel> GetQuestionWithAnswerAsync(int questionId, int answerId)
        {
            return await QueryAsync<QuestionModel>(HttpMethod.Get, $"Questions/{questionId}/{answerId}");
        }

        public async Task<CommonListResultModel<EasyToGetWrongQuestionModel>> GetEasyToGetWrongQuestions(int classId)
        {
            return await QueryAsync<CommonListResultModel<EasyToGetWrongQuestionModel>>(HttpMethod.Get, $"Questions/EasyToGetWrongQuestionsByClass/{classId}");
        }
    }
}
