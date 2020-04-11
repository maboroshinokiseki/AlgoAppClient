using AlgoApp.Models.Data;
using AlgoApp.Services;
using AlgoApp.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views.Teacher
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddStudentToClassPage : ContentPage
    {
        private readonly IAppServer appServer;

        private ViewModel VM { get; }

        public AddStudentToClassPage()
        {
            InitializeComponent();
        }

        public AddStudentToClassPage(int classId, ClassRoomPage.ViewModel model) : this()
        {
            this.appServer = DependencyService.Get<IAppServer>();
            this.VM = new ViewModel();

            VM.AddStudentCommand = new Command<ListItem>(async item =>
            {
                await appServer.AddStudentToClass(item.Id, classId);
                item.IsAdded = true;
                model.AddedStudent = true;
            });

            VM.SearchStudentCommand = new Command<string>(async searchText =>
            {
                var users = await appServer.SearchStudentsNotInClass(classId, searchText);
                if (users.Items.Count == 0)
                {
                    await DisplayAlert("", "未找到任何用户", "OK");
                    return;
                }

                VM.Items = new ObservableCollection<ListItem>(users.Items.Select(u => new ListItem { Id = u.Id, Text = u.Nickname, IsAdded = false }));
            });

            this.BindingContext = this.VM;
        }

        private async void MyListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is UserModel user))
                return;

            await Navigation.PushAsync(new StudentDetailTabbedPage(user.Id));
        }

        class ListItem : BaseViewModel
        {
            private bool isAdded;

            public int Id { get; set; }
            public string Text { get; set; }
            public bool IsAdded
            {
                get => isAdded;
                set => SetValue(out isAdded, value);
            }
        }

        class ViewModel : CommonListViewViewModel<ListItem>
        {
            public Command<ListItem> AddStudentCommand { get; set; }
            public Command<string> SearchStudentCommand { get; set; }
        }
    }
}