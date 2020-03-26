using AlgoApp.Models;
using AlgoApp.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views.Teacher
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        private readonly IAppServer appServer;

        public MainPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
            appServer = DependencyService.Get<IAppServer>();
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(e.SelectedItem is MasterMenuItemModel item))
                return;
            if (item.TargetType != null)
            {
                var page = (Page)Activator.CreateInstance(item.TargetType);
                page.Title = item.Title;

                Detail = new NavigationPage(page);
                IsPresented = false;
            }
            else if (item.Action != null)
            {
                item.Action.Invoke();
            }
            else if (item.Title == "退出")
            {
                await appServer.LogoutAsync();
                Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new NavigationPage(new LoginPage()));
            }

            MasterPage.ListView.SelectedItem = null;
        }
    }
}