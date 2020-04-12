using AlgoApp.Models.Data;
using AlgoApp.Services;
using AlgoApp.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnswerHistoryListPage : ContentPage
    {
        private readonly IAppServer appServer;
        private readonly Task<CommonListResultModel<HistoryItemModel>> historyTask;
        private readonly CommonListViewViewModel<ListModel> VM;

        private AnswerHistoryListPage()
        {
            InitializeComponent();
        }

        public AnswerHistoryListPage(int uid, int cid) : this()
        {
            appServer = DependencyService.Get<IAppServer>();
            historyTask = appServer.GetUserAnswerHistory(uid, cid);

            VM = new CommonListViewViewModel<ListModel>
            {
                Items = new ObservableCollection<ListModel>()
            };

            BindingContext = VM;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (VM.Items.Count != 0)
            {
                return;
            }

            foreach (var item in (await historyTask).Items)
            {
                var source = item.Correct ? ImageSource.FromFile("ic_action_check.png") : ImageSource.FromFile("ic_icon_wrong.png");
                VM.Items.Add(new ListModel { AnswerId = item.AnswerId, QuestionId = item.QuestionId, QuestionContent = item.QuestionContent, ImageSource = source });
            }
        }

        private async void MyListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is ListModel model))
                return;

            var history = await historyTask;
            var questionIds = history.Items.Select(i => i.QuestionId).ToList();
            var answerIds = history.Items.Select(i => i.AnswerId).ToList();
            await Navigation.PushAsync(new QuestionPage(model.QuestionId, questionIds, model.AnswerId, answerIds));
        }
    }

    public class ListModel
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public string QuestionContent { get; set; }
        public ImageSource ImageSource { get; set; }
    }
}