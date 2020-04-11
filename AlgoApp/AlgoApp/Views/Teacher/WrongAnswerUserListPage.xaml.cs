using AlgoApp.Models.Data;
using AlgoApp.Services;
using AlgoApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views.Teacher
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WrongAnswerUserListPage : ContentPage
    {
        private readonly IAppServer appServer;

        public BaseListViewModel<EasyToGetWrongQuestionModel> VM { get; }

        private readonly Task<CommonListResultModel<EasyToGetWrongQuestionModel>> easyToGetWrongQuestionsTask;
        public WrongAnswerUserListPage()
        {
            InitializeComponent();
        }

        public WrongAnswerUserListPage(int questionId):this()
        {
            appServer = DependencyService.Get<IAppServer>();
            VM = new BaseListViewModel<EasyToGetWrongQuestionModel> ();
            easyToGetWrongQuestionsTask = appServer.GetEasyToGetWrongQuestionsByQuestion(questionId);
            BindingContext = VM;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            VM.Items = new ObservableCollection<EasyToGetWrongQuestionModel>((await easyToGetWrongQuestionsTask).Items);
        }
    }
}