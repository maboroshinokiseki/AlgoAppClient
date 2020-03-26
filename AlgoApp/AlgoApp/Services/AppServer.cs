using AlgoApp.Models.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AlgoApp.Services
{
    class AppServer : IAppServer
    {
        private readonly HttpClient httpClient;
        private readonly HttpClientHandler handler;
        public AppServer()
        {
            domain = Preferences.Get("Domain", "10.0.2.2:5000");

            var cookieContainer = new CookieContainer();
            LoadCookies();
            handler = new HttpClientHandler { AllowAutoRedirect = true, UseCookies = true, CookieContainer = cookieContainer };
            httpClient = new HttpClient(handler)
            {
                BaseAddress = this.BaseAddress,
                Timeout = new TimeSpan(0, 0, 100)
            };
        }

        public async Task<LoginResultModel> LoginAsync(string username, string password)
        {
            //handler.CookieContainer = new CookieContainer();
            var json = JsonConvert.SerializeObject(new { username, password });
            var result = await QueryAsync<LoginResultModel>(HttpMethod.Post, "user/login", json);

            SaveCookies();

            return result;
        }

        public async Task<CommonResultModel> RegisterAsync(string username, string password)
        {
            //handler.CookieContainer = new CookieContainer();
            var json = JsonConvert.SerializeObject(new { username, password });
            var result = await QueryAsync<CommonResultModel>(HttpMethod.Post, "user/register", json);

            SaveCookies();

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

        private void SaveCookies()
        {
            Preferences.Set("Cookies", JsonConvert.SerializeObject(handler.CookieContainer.GetCookies(BaseAddress)));
        }

        private void LoadCookies()
        {
            if (!Preferences.ContainsKey("Cookies"))
            {
                return;
            }

            foreach (var cookie in JsonConvert.DeserializeObject<List<Cookie>>(Preferences.Get("Cookies", "[]")))
            {
                handler.CookieContainer.Add(new Cookie
                {
                    Name = cookie.Name,
                    Value = cookie.Value,
                    Path = cookie.Path,
                    Domain = cookie.Domain,
                    Expired = cookie.Expired,
                    Expires = cookie.Expires,
                });
            }
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
                return JsonConvert.DeserializeObject<T>(responseContent);
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

        public async Task<CommonResultModel> LogoutAsync()
        {
            var result = await QueryAsync<CommonResultModel>(HttpMethod.Get, "User/Logout");
            //handler.CookieContainer = new CookieContainer();
            SaveCookies();
            return result;
        }

        public async Task<ClassRoomListModel> MyClassRooms()
        {
            return await QueryAsync<ClassRoomListModel>(HttpMethod.Get, "ClassRoom/MyClassRooms");
        }
    }
}
