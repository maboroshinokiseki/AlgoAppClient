using AlgoApp.Models.Data;
using AlgoApp.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AlgoApp.Views.Student
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

            if (question.Status == QuestionStatus.Untouched)
            {
                OptionsListStackLayout.IsVisible = true;
                OptionButton1.Text = question.Options[0].Content;
                OptionButton2.Text = question.Options[1].Content;
                OptionButton3.Text = question.Options[2].Content;
                OptionButton4.Text = question.Options[3].Content;
            }
            else
            {
                DisplayAnswer(question.AnswerResult);
            }
        }

        private async void OptionButton_Clicked(object sender, EventArgs e)
        {
            QuestionModel.Option option;
            if (sender == OptionButton1)
                option = question.Options[0];
            else if (sender == OptionButton2)
                option = question.Options[1];
            else if (sender == OptionButton3)
                option = question.Options[2];
            else
                option = question.Options[3];

            var result = await appServer.PostAnswerAsync(question.Id, option.Id);
            DisplayAnswer(result);
        }

        private void DisplayAnswer(AnswerResultModel answer)
        {
            OptionsListStackLayout.IsVisible = false;
            AnalysisStackLayout.IsVisible = true;
            YourAnswerLabel.Text = answer.UserAnswer;
            if (answer.Correct)
            {
                YourAnswerLabel.TextColor = Color.Green;
            }
            else
            {
                YourAnswerLabel.TextColor = Color.Red;
            }

            CorrectAnswerLabel.Text = answer.CorrectAnswer;
            AnalysisLabel.Text = answer.Analysis;
        }
    }
}