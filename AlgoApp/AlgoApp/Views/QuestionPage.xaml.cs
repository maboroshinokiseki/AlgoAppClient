using AlgoApp.Models.Data;
using AlgoApp.Services;
using AlgoApp.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AlgoApp.Views
{
    public partial class QuestionPage : ContentPage
    {
        private QuestionModel question;
        private readonly int questionId;
        private readonly PageType pageType;
        private readonly int answerId;
        private readonly ViewModel VM;
        private readonly IAppServer appServer;

        private QuestionPage()
        {
            InitializeComponent();
        }

        public QuestionPage(int questionId, List<int> questionIds = null, int answerId = 0, List<int> answerIds = null, PageType pageType = PageType.Normal) : this()
        {
            appServer = DependencyService.Get<IAppServer>();
            this.questionId = questionId;
            this.pageType = pageType;
            this.answerId = answerId;
            VM = new ViewModel
            {
                PostAnswerCommand = new Command<QuestionModel.Option>(async o =>
                {
                    question.AnswerResult = await appServer.PostAnswerAsync(question.Id, o.Id, false);
                    DisplayAnswer(question.AnswerResult);
                }),
                QuestionReportCommand = new Command(async () => await Navigation.PushAsync(new QuestionReportPage(question.Id))),
                UserAnswerColor = Color.White
            };
            VM.HasMoreQuestions = true;

            if (questionIds != null)
            {
                VM.IsinList = true;
                VM.CurrentIndex = questionIds.IndexOf(questionId);
                
                if (answerIds != null)
                {
                    VM.PrevQuestionCommand = new Command(async () => await DisplayQuestion(questionIds[--VM.CurrentIndex], answerIds[VM.CurrentIndex]), () => VM.CurrentIndex != 0);
                    VM.NextQuestionCommand = new Command(async () => await DisplayQuestion(questionIds[++VM.CurrentIndex], answerIds[VM.CurrentIndex]), () => VM.CurrentIndex != questionIds.Count - 1);
                }
                else
                {
                    VM.PrevQuestionCommand = new Command(async () => await DisplayQuestion(questionIds[--VM.CurrentIndex]), () => VM.CurrentIndex != 0);
                    VM.NextQuestionCommand = new Command(async () => await DisplayQuestion(questionIds[++VM.CurrentIndex]), () => VM.CurrentIndex != questionIds.Count - 1);
                }
            }

            if (pageType == PageType.DailyPractice)
            {
                VM.PostAnswerCommand = new Command<QuestionModel.Option>(async o =>
                {
                    question.AnswerResult = await appServer.PostAnswerAsync(question.Id, o.Id, true);
                    DisplayAnswer(question.AnswerResult);
                    VM.Answered = true;
                });
                VM.IsInOneWayQuestMode = true;
                VM.NoMoreQuestionText = "每日练习已完成";
                VM.NextQuestionCommand = new Command(async () => await DisplayQuestion(0, 0, pageType), () => VM.Answered == true);
            }
            else if (pageType == PageType.BreakThroughMode)
            {
                VM.PostAnswerCommand = new Command<QuestionModel.Option>(async o =>
                {
                    question.AnswerResult = await appServer.PostAnswerAsync(question.Id, o.Id, false);
                    DisplayAnswer(question.AnswerResult);
                    VM.Answered = true;
                });
                VM.IsInOneWayQuestMode = true;
                VM.NoMoreQuestionText = "已完成所有题目";
                VM.NextQuestionCommand = new Command(async () => await DisplayQuestion(0, 0, pageType), () => VM.Answered == true);
            }

            VM.BookmarkCommand = new Command(async () =>
            {
                if (VM.IsInBookmark)
                {
                    await appServer.RemoveQuestionFromBookmark(VM.QuestionId);
                    VM.BookmarkText = "加入书签";
                }
                else
                {
                    await appServer.AddQuestionToBookmark(VM.QuestionId);
                    VM.BookmarkText = "从书签移除";
                }
            });

            BindingContext = VM;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await DisplayQuestion(questionId, answerId, pageType);
        }

        private async Task DisplayQuestion(int questionId, int answerId = 0, PageType pageType = PageType.Normal)
        {
            if (pageType == PageType.Normal)
            {
                if (answerId == 0)
                {
                    question = await appServer.GetQuestionAsync(questionId);
                }
                else
                {
                    question = await appServer.GetQuestionWithAnswerAsync(questionId, answerId);
                }

                VM.ShowAnswers = false;
                VM.ShowOptions = false;

                if (question.AnswerResult == null)
                {
                    VM.ShowOptions = true;
                }
                else
                {
                    DisplayAnswer(question.AnswerResult);
                }
            }
            else if (pageType == PageType.DailyPractice)
            {
                question = await appServer.GetDailyPracticeQuestion();

                VM.ShowAnswers = false;
                VM.ShowOptions = true;
                VM.Answered = false;
            }
            else if (pageType == PageType.BreakThroughMode)
            {
                question = await appServer.GetBreakThroughQuestion();

                VM.ShowAnswers = false;
                VM.ShowOptions = true;
                VM.Answered = false;
            }

            if (question.Code == Codes.NoMoreQuestions)
            {
                VM.NoMoreQuestions = true;
                VM.HasMoreQuestions = false;
                ToolbarItems.Clear();
                return;
            }

            VM.QuestionContent = question.Content;
            VM.Items = new ObservableCollection<QuestionModel.Option>(question.Options);

            var inBookmark = await appServer.IsQuestionInBookmark(question.Id);
            switch (inBookmark.Code)
            {
                case Codes.QuestionInBookmark:
                    VM.BookmarkText = "从书签移除";
                    VM.IsInBookmark = true;
                    break;
                case Codes.QuestionNotInBookmark:
                    VM.BookmarkText = "加入书签";
                    VM.IsInBookmark = false;
                    break;
                default:
                    break;
            }
            VM.QuestionId = question.Id;
        }

        private void DisplayAnswer(AnswerResultModel answer)
        {
            VM.ShowOptions = false;
            VM.ShowAnswers = true;

            VM.OptionsString = string.Join("、", question.Options.Select(o => o.Content));
            VM.HaveUserAnswer = answer.UserAnswer != null;
            VM.UserAnswerColor = answer.Correct ? Color.Green : Color.Red;
            VM.UserAnswer = answer.UserAnswer;
            VM.CorrectAnswer = answer.CorrectAnswer;
            VM.Analysis = question.Analysis;
        }

        class ViewModel : BaseListViewModel<QuestionModel.Option>
        {
            public int QuestionId;
            public bool IsInBookmark;

            private string bookmarkText;

            public string BookmarkText
            {
                get => bookmarkText;
                set => SetValue(out bookmarkText, value);
            }

            private string questionContent;

            public string QuestionContent
            {
                get => questionContent;
                set => SetValue(out questionContent, value);
            }

            private bool showAnswers;

            public bool ShowAnswers
            {
                get => showAnswers;
                set => SetValue(out showAnswers, value);
            }

            private bool showOptions;

            public bool ShowOptions
            {
                get => showOptions;
                set => SetValue(out showOptions, value);
            }

            private string optionsString;
            public string OptionsString
            {
                get => optionsString;
                set => SetValue(out optionsString, value);
            }

            private string userAnswer;
            public string UserAnswer
            {
                get => userAnswer;
                set => SetValue(out userAnswer, value);
            }

            private string correctAnswer;
            public string CorrectAnswer
            {
                get => correctAnswer;
                set => SetValue(out correctAnswer, value);
            }

            private string analysis;
            public string Analysis
            {
                get => analysis;
                set => SetValue(out analysis, value);
            }

            private Color userAnswerColor;
            public Color UserAnswerColor
            {
                get => userAnswerColor;
                set => SetValue(out userAnswerColor, value);
            }

            private int currentIndex;

            public int CurrentIndex
            {
                get => currentIndex;
                set
                {
                    SetValue(out currentIndex, value);
                    PrevQuestionCommand?.ChangeCanExecute();
                    NextQuestionCommand?.ChangeCanExecute();
                }
            }

            private bool haveUserAnswer;

            public bool HaveUserAnswer
            {
                get => haveUserAnswer;
                set => SetValue(out haveUserAnswer, value);
            }

            private bool isInList;

            public bool IsinList
            {
                get => isInList;
                set => SetValue(out isInList, value);
            }

            private bool isInOneWayQuestMode;

            public bool IsInOneWayQuestMode
            {
                get => isInOneWayQuestMode;
                set => SetValue(out isInOneWayQuestMode, value);
            }

            private string noMoreQuestionText;

            public string NoMoreQuestionText
            {
                get => noMoreQuestionText;
                set => SetValue(out noMoreQuestionText, value);
            }

            private bool answered;

            public bool Answered
            {
                get => answered;
                set
                {
                    SetValue(out answered, value);
                    NextQuestionCommand.ChangeCanExecute();
                }
            }

            private bool hasMoreQuestions;

            public bool HasMoreQuestions
            {
                get => hasMoreQuestions;
                set => SetValue(out hasMoreQuestions, value);
            }

            private bool noMoreQuestions;

            public bool NoMoreQuestions
            {
                get => noMoreQuestions;
                set => SetValue(out noMoreQuestions, value);
            }

            public Command<QuestionModel.Option> PostAnswerCommand { get; set; }
            public Command PrevQuestionCommand { get; set; }
            public Command NextQuestionCommand { get; set; }
            public Command BookmarkCommand { get; set; }
            public Command QuestionReportCommand { get; set; }
        }

        public enum PageType
        {
            Normal,
            DailyPractice,
            BreakThroughMode,
        }
    }
}