using AlgoApp.Models.Data;
using AlgoApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views.Teacher
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClassRoomListPage : ContentPage
    {
        private readonly IAppServer appServer;
        public ObservableCollection<ClassRoomModel> Items { get; set; }

        public ClassRoomListPage()
        {
            InitializeComponent();

            appServer = DependencyService.Get<IAppServer>();

            Items = new ObservableCollection<ClassRoomModel>();

            MyListView.ItemsSource = Items;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Items.Count != 0)
            {
                return;
            }

            MyListView.IsRefreshing = true;
            var classRooms = await appServer.MyClassRooms();
            if (classRooms.Items == null)
            {
                MyListView.IsRefreshing = false;
                return;
            }

            foreach (var item in classRooms.Items)
            {
                Items.Add(item);
            }
            await Task.Delay(1000);
            MyListView.IsRefreshing = false;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Items.Clear();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is ClassRoomModel classRoom))
                return;

            await Navigation.PushAsync(new ClassRoomPage(classRoom.Id) { Title = classRoom.Name });

        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            string name;
            while (true)
            {
                name = await DisplayPromptAsync("班級名字", "");
                if (string.IsNullOrWhiteSpace(name))
                {
                    if (name != null)
                    {
                        await DisplayAlert("錯誤", "請輸入名字", "確認");
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

            var newClass = await appServer.AddClassRoom(name.Trim());
            Items.Add(newClass);
        }
    }
}
