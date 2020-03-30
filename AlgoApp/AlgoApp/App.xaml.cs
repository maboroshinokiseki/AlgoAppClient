using AlgoApp.Models.Data;
using AlgoApp.Services;
using AlgoApp.Views;
using Newtonsoft.Json;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DependencyService.Register<AppServer>();
            MainPage = new NavigationPage(new LoginPage());
        }

        protected async override void OnStart()
        {
            var appServer = DependencyService.Get<IAppServer>();
            var res = await appServer.GetCurrentUserAsync();
            if (res.Code == Codes.None)
            {
                if (res.Role == UserRole.Student)
                {
                    MainPage = new Views.Student.MainPage();
                }
                else if (res.Role == UserRole.Teacher)
                {
                    MainPage = new Views.Teacher.MainPage();
                }
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
