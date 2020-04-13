using AlgoApp.Models.Data;
using AlgoApp.Services;
using AlgoApp.ViewModels;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views.Teacher
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClassWrongAnswerListPage : ContentPage
    {
        private readonly int classId;
        private readonly int questionId;
        private readonly IAppServer appServer;

        public CommonListViewViewModel<EasyToGetWrongQuestionModel> VM { get; }

        private ClassWrongAnswerListPage()
        {
            InitializeComponent();
        }

        public ClassWrongAnswerListPage(int classId, int questionId) : this()
        {
            this.classId = classId;
            this.questionId = questionId;
            appServer = DependencyService.Get<IAppServer>();
            VM = new CommonListViewViewModel<EasyToGetWrongQuestionModel>();
            BindingContext = VM;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (VM.Items != null)
            {
                return;
            }

            VM.IsBusy = true;
            VM.Items = new ObservableCollection<EasyToGetWrongQuestionModel>((await appServer.GetEasyToGetWrongQuestionDetail(classId, questionId)).Items);
            VM.IsBusy = false;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}