using AlgoApp.Models.Data;
using AlgoApp.Services;
using System;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views.Teacher
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddStudentToClassPage : ContentPage
    {
        private readonly int classId;
        private readonly IAppServer appServer;
        public ObservableCollection<UserModel> Items { get; set; }
        public Command AddStudentCommand { get; }

        public AddStudentToClassPage()
        {
            InitializeComponent();
        }

        public AddStudentToClassPage(int classId) : this()
        {
            this.classId = classId;
            this.appServer = DependencyService.Get<IAppServer>();
            Items = new ObservableCollection<UserModel>();
            this.BindingContext = this;

            AddStudentCommand = new Command(async item =>
            {
                var u = item as UserModel;
                await appServer.AddStudentToClass(u.Id, classId);
            });
        }

        private async void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            Items.Clear();
            var users = await appServer.SearchStudentsNotInClass(classId, nameSearchBar.Text);
            if (users.Items.Count == 0)
            {
                await DisplayAlert("", "未找到任何用户", "OK");
                return;
            }

            foreach (var u in users.Items)
            {
                Items.Add(u);
            }
        }

        private async void MyListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is UserModel user))
                return;

            await Navigation.PushAsync(new StudentDetailTabbedPage(user.Id));
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var b = sender as ImageButton;
            var p = b.Parent as StackLayout;
            b.IsVisible = false;
            p.Children[2].IsVisible = true;
        }
    }
}