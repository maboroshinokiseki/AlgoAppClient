using AlgoApp.Models.Data;
using AlgoApp.Services;
using AlgoApp.ViewModels;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views.Teacher
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClassWrongAnswerQuestionListPage : ContentPage
    {
        private readonly int classId;
        private readonly int chapterId;
        private readonly IAppServer appServer;

        public CommonListViewViewModel<EasyToGetWrongQuestionModel> VM { get; }

        private ClassWrongAnswerQuestionListPage()
        {
            InitializeComponent();
        }

        public ClassWrongAnswerQuestionListPage(int classId, int chapterId) : this()
        {
            this.classId = classId;
            this.chapterId = chapterId;
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
            VM.Items = new ObservableCollection<EasyToGetWrongQuestionModel>((await appServer.GetEasyToGetWrongQuestionsByClassChapter(classId, chapterId)).Items);
            VM.IsBusy = false;
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is EasyToGetWrongQuestionModel model))
                return;

            await Navigation.PushAsync(new ClassWrongAnswerDetailTabbedPage(this.classId, model.QuestionId));
        }
    }
}