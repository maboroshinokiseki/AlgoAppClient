using AlgoApp.Models.Data;
using AlgoApp.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly IAppServer appServer;
        private readonly Task<UserModel> GetCurrentUserTask;

        public LoginPage()
        {
            InitializeComponent();
            if (!DesignMode.IsDesignModeEnabled)
            {
                appServer = DependencyService.Get<IAppServer>();
            }

            GetCurrentUserTask = appServer.GetCurrentUserAsync();
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoadingPage(), false);
            var res = await appServer.LoginAsync(usernameEntry.Text, passwordEntry.Text);
            switch (res.Code)
            {
                case Codes.Unknown:
                    await DisplayAlert("錯誤", "未知錯誤", "確定");
                    break;
                case Codes.None:
                    if (res.Role == "Student")
                    {
                        Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new Student.MainPage());
                    }
                    else if (res.Role == "Teacher")
                    {
                        Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new Teacher.MainPage());
                    }
                    break;
                case Codes.LoginFailed:
                    await DisplayAlert("錯誤", "用戶名或密碼錯誤", "確定");
                    break;
                case Codes.TimeOut:
                    await DisplayAlert("超時", "請檢查網絡連接", "確定");
                    break;
                default:
                    break;
            }

            Device.BeginInvokeOnMainThread(async () => await Navigation.PopModalAsync(false));
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            var newDomain = await DisplayPromptAsync("Server Location", "", initialValue: appServer.Domain);

            var temp = newDomain.Split(':');
            if (temp.Length > 2 ||
                temp.Length == 2 && !int.TryParse(temp[1], out _) ||
                Uri.CheckHostName(temp[0]) == UriHostNameType.Unknown)
            {
                await DisplayAlert("Error", "Invalid Domain", "OK");
            }
            else
            {
                appServer.Domain = newDomain;
            }
        }
    }
}