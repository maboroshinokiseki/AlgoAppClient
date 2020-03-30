using AlgoApp.Models.Data;
using AlgoApp.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views.Teacher
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClassRoomListPage : ContentPage
    {
        private readonly IAppServer appServer;
        private readonly Task<ClassRoomListModel> classroomsTask;

        public ObservableCollection<ClassRoomModel> Items { get; set; }

        public ClassRoomListPage()
        {
            InitializeComponent();

            appServer = DependencyService.Get<IAppServer>();

            Items = new ObservableCollection<ClassRoomModel>();

            MyListView.ItemsSource = Items;

            classroomsTask = appServer.MyClassRooms();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Items.Count != 0)
            {
                return;
            }

            MyListView.IsRefreshing = true;
            var classRooms = await classroomsTask;
            if (classRooms.ClassRooms == null)
            {
                MyListView.IsRefreshing = false;
                return;
            }

            foreach (var item in classRooms.ClassRooms)
            {
                Items.Add(item);
            }
            await Task.Delay(1000);
            MyListView.IsRefreshing = false;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is ClassRoomModel classRoom))
                return;

            await Navigation.PushAsync(new ClassRoomPage(classRoom.Id));

        }
    }
}
