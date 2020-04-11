using AlgoApp.Models.Data;
using AlgoApp.Services;
using AlgoApp.Utilities;
using AlgoApp.ViewModels;
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

            await Navigation.PushAsync(new ClassRoomPage(classRoom.Id) { Title = classRoom.Name });
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            string name = await UIUtilities.DisplayPromptAsync(this, "新建班级", "请输入新班级名称", "请输入名字");
            if (name == null)
            {
                return;
            }

            var newClass = await appServer.AddClassRoom(name.Trim());
            VM.Items.Add(newClass);
        }
    }
}
