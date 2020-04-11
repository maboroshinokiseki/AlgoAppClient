using AlgoApp.Models;
using AlgoApp.Services;
using AlgoApp.ViewModels;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views.Student
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageMaster : ContentPage
    {
        private readonly MasterDetailPage master;

        private MainPageMaster()
        {
            InitializeComponent();
        }

        public MainPageMaster(MasterDetailPage master) : this()
        {
            BindingContext = new MainPageMasterViewModel(master);
            this.master = master;
        }

        class MainPageMasterViewModel : BaseListViewModel<MasterMenuItemModel>
        {
            private string name;
            public string Name
            {
                get => name;
                set => SetValue(out name, value);
            }


            public MainPageMasterViewModel(MasterDetailPage master)
            {
                var appServer = DependencyService.Get<IAppServer>();
                Items = new ObservableCollection<MasterMenuItemModel>(new[]
                {
                    new MasterMenuItemModel { Id = 0, Title = "个人信息", Action = () => master.Detail = new NavigationPage(new ProfilePage(App.UserId, true)) },
                    new MasterMenuItemModel { Id = 1, Title = "章節列表", Action = () => master.Detail = new NavigationPage(new ChapterListPage()) },
                    new MasterMenuItemModel { Id = 2, Title = "收藏夹", Action = () => master.Detail = new NavigationPage(new QuestionListPage(0, true) { Title = "收藏夹" }) },
                    new MasterMenuItemModel { Id = 3, Title = "班级列表", Action = () => master.Detail = new NavigationPage(new ClassRoomListPage()) },
                    new MasterMenuItemModel { Id = 4, Title = "每日一练", Action = () => master.Detail = new NavigationPage(new QuestionPage(0, pageType: QuestionPage.PageType.DailyPractice) { Title = "每日一练" }) },
                    new MasterMenuItemModel { Id = 5, Title = "闯关答题", Action = () => master.Detail = new NavigationPage(new QuestionPage(0, pageType: QuestionPage.PageType.BreakThroughMode) { Title = "闯关答题" }) },
                    new MasterMenuItemModel { Id = 6, Title = "退出", Action = () => {
                        appServer.Logout();
                        Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new NavigationPage(new LoginPage()));
                    }},
                });

                appServer.GetCurrentUserAsync().ContinueWith(u => Name = u.Result.Nickname);
            }
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is MasterMenuItemModel item))
                return;

            master.IsPresented = false;
            if (item.Action != null)
            {
                item.Action.Invoke();
            }
        }
    }
}