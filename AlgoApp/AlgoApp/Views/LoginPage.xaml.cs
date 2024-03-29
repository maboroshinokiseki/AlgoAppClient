﻿using AlgoApp.Models.Data;
using AlgoApp.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly IAppServer appServer;

        public LoginPage()
        {
            InitializeComponent();
            if (!DesignMode.IsDesignModeEnabled)
            {
                appServer = DependencyService.Get<IAppServer>();
            }
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoadingPage(), false);
            var res = await appServer.LoginAsync(usernameEntry.Text, passwordEntry.Text);
            switch (res.Code)
            {
                case Codes.Unknown:
                    await DisplayAlert("错误", "未知错误", "确定");
                    break;
                case Codes.None:
                    App.UserId = res.UserId;
                    App.Role = Enum.Parse<UserRole>(res.Role);
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
                    await DisplayAlert("错误", "用户名或密码错误", "确定");
                    break;
                case Codes.TimeOut:
                    await DisplayAlert("超时", "请检查网络连接", "确定");
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
            string newDomain;
            while (true)
            {
                newDomain = await DisplayPromptAsync("Server Location", "", initialValue: appServer.Domain);
                if (string.IsNullOrWhiteSpace(newDomain))
                {
                    if (newDomain != null)
                    {
                        await DisplayAlert("错误", "非法地址", "确认");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    break;
                }
            }

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