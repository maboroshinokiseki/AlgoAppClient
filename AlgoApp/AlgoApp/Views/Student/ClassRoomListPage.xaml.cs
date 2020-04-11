using AlgoApp.Models.Data;
using AlgoApp.Services;
using AlgoApp.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views.Student
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClassRoomListPage : ContentPage
    {
        private readonly IAppServer appServer;

        internal CommonListViewViewModel<ClassRoomModel> VM { get; }

        public ClassRoomListPage()
        {
            InitializeComponent();

            appServer = DependencyService.Get<IAppServer>();
            VM = new CommonListViewViewModel<ClassRoomModel>();
            BindingContext = VM;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            VM.IsBusy = true;
            var classRooms = await appServer.MyClassRooms();
            if (classRooms.Items == null)
            {
                VM.IsBusy = false;
                return;
            }

            VM.Items = new ObservableCollection<ClassRoomModel>(classRooms.Items);
            await Task.Delay(500);
            VM.IsBusy = false;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is ClassRoomModel classRoom))
                return;

            //await Navigation.PushAsync(new ClassRoomPage(classRoom.Id) { Title = classRoom.Name });
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new JoinClassPage());
        }
    }
}
