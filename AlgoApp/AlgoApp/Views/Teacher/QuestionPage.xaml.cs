using AlgoApp.Models.Data;
using AlgoApp.Services;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AlgoApp.Views.Teacher
{
    public partial class QuestionPage : ContentPage
    {
        private readonly Task<QuestionModel> questionTask;
        private readonly IAppServer appServer;

        public QuestionPage()
        {
            InitializeComponent();

            appServer = DependencyService.Get<IAppServer>();
        }

        public QuestionPage(int questionId) : this()
        {
            questionTask = appServer.GetQuestionAsync(questionId);
        }

        private QuestionModel question;

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            question = await questionTask;

            ContentLabel.Text = question.Content;

            AnswerListLabel.Text = string.Join("、", question.Options.Select(o => o.Content));
            CorrectAnswerLabel.Text = question.AnswerResult.CorrectAnswer;
            AnalysisLabel.Text = question.AnswerResult.Analysis;
        }
    }
}