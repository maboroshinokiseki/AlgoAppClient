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
                await DisplayAlert("錯誤", "兩次輸入密碼不一致", "確定");
                return;
            }

            await Navigation.PushModalAsync(new LoadingPage(), false);
            var result = await appServer.RegisterAsync(usernameEntry.Text, passwordEntry.Text);
            switch (result.Code)
            {
                case Codes.Unknown:
                    await DisplayAlert("錯誤", "未知錯誤", "確定");
                    break;
                case Codes.None:
                    App.UserId = result.UserId;
                    Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new Student.MainPage());
                    break;
                case Codes.RegistrationFailed:
                    await DisplayAlert("錯誤", "用戶名已存在", "確定");
                    break;
                case Codes.TimeOut:
                    await DisplayAlert("超時", "請檢查網絡連接", "確定");
                    break;
                default:
                    break;
            }

            Device.BeginInvokeOnMainThread(async () => await Navigation.PopModalAsync(false));
        }
    }
}