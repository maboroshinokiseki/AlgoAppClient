using AlgoApp.Models.Data;
using AlgoApp.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AlgoApp.Views.Student
{
    public partial class QuestionListPage : ContentPage
    {
        public ObservableCollection<QuestionModel> Items { get; set; }

        public QuestionListPage()
        {
            InitializeComponent();

            appServer = DependencyService.Get<IAppServer>();

            Items = new ObservableCollection<QuestionModel>();

            BindingContext = this;
        }

        private readonly Task<CommonListResultModel<QuestionModel>> questionsTask;
        private readonly IAppServer appServer;

        public QuestionListPage(int chapterId) : this()
        {
            questionsTask = appServer.GetQuestionListAsync(chapterId);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Items.Count != 0)
            {
                return;
            }

            MyListView.IsRefreshing = true;
            var questions = await questionsTask;
            if (questions.Items == null)
            {
                MyListView.IsRefreshing = false;
                return;
            }

            foreach (var item in questions.Items)
            {
                Items.Add(item);
            }
            MyListView.IsRefreshing = false;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is QuestionModel question))
                return;

            await Navigation.PushAsync(new QuestionPage(question.Id));
        }
    }
}
