using AlgoApp.Models.Data;
using AlgoApp.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        private readonly IAppServer appServer;

        public RegisterPage()
        {
            InitializeComponent();

            appServer = DependencyService.Get<IAppServer>();
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            if (passwordEntry.Text != confirmPasswordEntry.Text)
            {
                await DisplayAlert("错误", "两次输入密码不一致", "确定");
                return;
            }

            await Navigation.PushModalAsync(new LoadingPage(), false);
            var result = await appServer.RegisterAsync(usernameEntry.Text, passwordEntry.Text);
            switch (result.Code)
            {
                case Codes.Unknown:
                    await DisplayAlert("错误", "未知错误", "确定");
                    break;
                case Codes.None:
                    App.UserId = result.UserId;
                    App.Role = UserRole.Student;
                    Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new Student.MainPage());
                    break;
                case Codes.RegistrationFailed:
                    await DisplayAlert("错误", "用户名已存在", "确定");
                    break;
                case Codes.TimeOut:
                    await DisplayAlert("超时", "请检查网络连接", "确定");
                    break;
                default:
                    break;
            }

            Device.BeginInvokeOnMainThread(async () => await Navigation.PopModalAsync(false));
        }
    }
}