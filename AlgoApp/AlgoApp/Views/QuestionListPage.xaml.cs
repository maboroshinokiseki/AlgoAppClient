using AlgoApp.Models.Data;
using AlgoApp.Services;
using AlgoApp.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AlgoApp.Views
{
    public partial class QuestionListPage : ContentPage
    {
        private readonly int chapterId;
        private readonly bool isBookmarkPage;
        private readonly IAppServer appServer;
        private readonly CommonListViewViewModel<QuestionModel> VM;

        private QuestionListPage()
        {
            InitializeComponent();
        }

        public QuestionListPage(int chapterId, bool isBookmarkPage = false) : this()
        {
            this.chapterId = chapterId;
            this.isBookmarkPage = isBookmarkPage;
            appServer = DependencyService.Get<IAppServer>();
            VM = new CommonListViewViewModel<QuestionModel>();
            BindingContext = VM;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            VM.IsBusy = true;
            CommonListResultModel<QuestionModel> questions;
            if (isBookmarkPage)
            {
                 questions = await appServer.QuestionsInBookmark();
            }
            else
            {
                questions = await appServer.GetQuestionListAsync(chapterId);
            }

            if (questions.Items == null)
            {
                VM.IsBusy = false;
                return;
            }
            VM.Items = new ObservableCollection<QuestionModel>(questions.Items);
            await Task.Delay(500);
            VM.IsBusy = false;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is QuestionModel question))
                return;

            await Navigation.PushAsync(new QuestionPage(question.Id, VM.Items.Select(q => q.Id).ToList()));
        }
    }
}
