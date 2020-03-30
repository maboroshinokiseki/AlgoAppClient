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

        public async Task<ChapterListModel> GetChaperListAsync()
        {
            return await QueryAsync<ChapterListModel>(HttpMethod.Get, "Chapters");
        }

        public async Task<QuestionListModel> GetQuestionListAsync(int chapterId)
        {
            var model = await QueryAsync<QuestionListModel>(HttpMethod.Get, $"Chapters/{chapterId}/questions");
            for (int i = 0; i < model.Questions.Count; i++)
            {
                model.Questions[i].ContentWithIndex = $"{i + 1}、{model.Questions[i].Content}";
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

        public async Task<ClassRoomListModel> MyClassRooms()
        {
            return await QueryAsync<ClassRoomListModel>(HttpMethod.Get, "ClassRoom/MyClassRooms");
        }

        public async Task<ClassRoomModel> ClassRoom(int id)
        {
            return await QueryAsync<ClassRoomModel>(HttpMethod.Get, $"ClassRoom/ClassRoom/{id}");
        }

        public async Task<UserModel> GetUserDetail(int id)
        {
            return await QueryAsync<UserModel>(HttpMethod.Get, $"User/UserDetail/{id}");
        }
    }
}
