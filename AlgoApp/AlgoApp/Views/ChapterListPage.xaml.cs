using AlgoApp.Models.Data;
using AlgoApp.Services;
using AlgoApp.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AlgoApp.Views
{
    public partial class ChapterListPage : ContentPage
    {
        private readonly CommonListViewViewModel<ChapterModel> VM;
        private readonly IAppServer appServer;
        private readonly Task<CommonListResultModel<ChapterModel>> chaptersTask;
        private PageType pageType;
        private int uid;

        private ChapterListPage()
        {
            InitializeComponent();

            appServer = DependencyService.Get<IAppServer>();
            VM = new CommonListViewViewModel<ChapterModel>();
            BindingContext = VM;
        }

        public ChapterListPage(PageType pageType = PageType.Normal, int uid = 0) : this()
        {
            this.pageType = pageType;
            this.uid = uid;
            switch (pageType)
            {
                case PageType.Normal:
                    chaptersTask = appServer.GetChaperListAsync();
                    break;
                case PageType.AnswerHistory:
                    chaptersTask = appServer.GetUserAnswerHistoryChapters(uid);
                    break;
                default:
                    break;
            }
        }

        public enum PageType
        {
            Normal,
            AnswerHistory,
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (VM.Items != null)
            {
                return;
            }

            VM.IsBusy = true;
            var chapters = await chaptersTask;
            if (chapters.Items == null)
            {
                VM.IsBusy = false;
                return;
            }

            VM.Items = new ObservableCollection<ChapterModel>(chapters.Items);
            await Task.Delay(500);
            VM.IsBusy = false;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is ChapterModel chapter))
                return;
            switch (pageType)
            {
                case PageType.Normal:
                    await Navigation.PushAsync(new QuestionListPage(chapter.Id) { Title = chapter.Name });
                    break;
                case PageType.AnswerHistory:
                    await Navigation.PushAsync(new AnswerHistoryListPage(uid, chapter.Id) { Title = chapter.Name });
                    break;
                default:
                    break;
            }
        }
    }
}
