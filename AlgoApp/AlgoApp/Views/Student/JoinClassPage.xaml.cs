using AlgoApp.Models.Data;
using AlgoApp.Services;
using AlgoApp.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views.Student
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JoinClassPage : ContentPage
    {
        private readonly IAppServer appServer;

        private ViewModel VM { get; }

        public JoinClassPage()
        {
            InitializeComponent();

            this.appServer = DependencyService.Get<IAppServer>();
            this.VM = new ViewModel();

            VM.JoinClassCommand = new Command<ListItem>(async item =>
            {
                await appServer.JoinClassRomm(item.Id);
                item.IsAdded = true;
            });

            VM.SearchClassCommand = new Command<string>(async searchText =>
            {
                var classRooms = await appServer.SearchClassImNotIn(searchText);
                if (classRooms.Items.Count == 0)
                {
                    await DisplayAlert("", "未找到任何班级", "OK");
                    return;
                }

                VM.Items = new ObservableCollection<ListItem>(classRooms.Items.Select(c => new ListItem { Id = c.Id, Text = c.Name, IsAdded = false }));
            });

            this.BindingContext = this.VM;
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
            public Command<ListItem> JoinClassCommand { get; set; }
            public Command<string> SearchClassCommand { get; set; }
        }
    }
}