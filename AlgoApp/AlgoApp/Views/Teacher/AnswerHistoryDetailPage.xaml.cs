using AlgoApp.Models.Data;
using AlgoApp.Services;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views.Teacher
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnswerHistoryDetailPage : ContentPage
    {
        private readonly IAppServer appServer;
        private readonly Task<QuestionModel> questionTask;

        public AnswerHistoryDetailPage()
        {
            InitializeComponent();
        }

        public AnswerHistoryDetailPage(int qid, int aid) : this()
        {
            appServer = DependencyService.Get<IAppServer>();
            questionTask = appServer.GetQuestionWithAnswerAsync(qid, aid);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var question = await questionTask;

            ContentLabel.Text = question.Content;

            AnswerListLabel.Text = string.Join("、", question.Options.Select(o => o.Content));
            CorrectAnswerLabel.Text = question.AnswerResult.CorrectAnswer;
            AnalysisLabel.Text = question.Analysis;
            YourAnswerLabel.Text = question.AnswerResult.UserAnswer;
            if (question.AnswerResult.Correct)
            {
                YourAnswerLabel.TextColor = Color.Green;
            }
            else
            {
                YourAnswerLabel.TextColor = Color.Red;
            }

        }
    }
}