using AlgoApp.Models.Data;
using AlgoApp.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AlgoApp.Views.Teacher
{
    public partial class ClassRoomPage : ContentPage
    {
        private readonly IAppServer appServer;
        private readonly Task<ClassRoomModel> classRommTask;

        public ObservableCollection<UserModel> Items { get; set; }

        public ClassRoomPage()
        {
            InitializeComponent();

            appServer = DependencyService.Get<IAppServer>();

            Items = new ObservableCollection<UserModel>();

            BindingContext = this;
        }

        public ClassRoomPage(int classId): this()
        {
            classRommTask = appServer.ClassRoom(classId);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Items.Count != 0)
            {
                return;
            }

            MyListView.IsRefreshing = true;
            var classRoom = await classRommTask;

            foreach (var item in classRoom.Students)
            {
                Items.Add(item);
            }

            MyListView.IsRefreshing = false;
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            
        }
    }
}
