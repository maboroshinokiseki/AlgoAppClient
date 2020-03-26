using AlgoApp.Models;
using AlgoApp.Services;
using AlgoApp.ViewModels;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views.Teacher
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageMaster : ContentPage
    {
        public ListView ListView;

        public MainPageMaster()
        {
            InitializeComponent();

            BindingContext = new MainPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MainPageMasterViewModel : BaseViewModel
        {
            public ObservableCollection<MasterMenuItemModel> MenuItems { get; set; }

            private string name;
            public string Name
            {
                get => name;
                set => SetValue(out name, value);
            }


            public MainPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MasterMenuItemModel>(new[]
                {
                    new MasterMenuItemModel { Id = 0, Title = "章節列表", TargetType = typeof(ChapterListPage) },
                    new MasterMenuItemModel { Id = 0, Title = "班級", TargetType = typeof(ClassRoomListPage) },
                    new MasterMenuItemModel { Id = 1, Title = "退出" },
                });

                var server = DependencyService.Get<IAppServer>();
                server.GetCurrentUserAsync().ContinueWith(u => Name = u.Result.NickName);
            }
        }
    }
}