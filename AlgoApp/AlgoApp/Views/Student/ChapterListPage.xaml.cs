using AlgoApp.Models.Data;
using AlgoApp.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AlgoApp.Views.Student
{
    public partial class ChapterListPage : ContentPage
    {
        private readonly IAppServer appServer;
        private readonly Task<ChapterListModel> chaptersTask;

        public ObservableCollection<ChapterModel> Items { get; set; }

        public ChapterListPage()
        {
            InitializeComponent();

            appServer = DependencyService.Get<IAppServer>();

            Items = new ObservableCollection<ChapterModel>();

            BindingContext = this;

            chaptersTask = appServer.GetChaperListAsync();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Items.Count != 0)
            {
                return;
            }

            MyListView.IsRefreshing = true;
            var chapters = await chaptersTask;
            if (chapters.Chapters == null)
            {
                MyListView.IsRefreshing = false;
                return;
            }

            foreach (var item in chapters.Chapters)
            {
                Items.Add(item);
            }
            await Task.Delay(1000);
            MyListView.IsRefreshing = false;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is ChapterModel chapter))
                return;

            await Navigation.PushAsync(new QuestionListPage(chapter.Id));
        }
    }
}
