using AlgoApp.Models.Data;
using AlgoApp.Services;
using AlgoApp.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views.Teacher
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WrongAnswerListPage : ContentPage
    {
        private readonly IAppServer appServer;

        public BaseListViewModel<EasyToGetWrongQuestionModel> VM { get; }

        private readonly Task<CommonListResultModel<EasyToGetWrongQuestionModel>> easyToGetWrongQuestionsTask;

        public WrongAnswerListPage()
        {
            InitializeComponent();
        }

        public WrongAnswerListPage(int classId) : this()
        {
            appServer = DependencyService.Get<IAppServer>();
            VM = new BaseListViewModel<EasyToGetWrongQuestionModel> { Items = new ObservableCollection<EasyToGetWrongQuestionModel>() };
            easyToGetWrongQuestionsTask = appServer.GetEasyToGetWrongQuestionsByClass(classId);
            BindingContext = VM;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (VM.Items.Count != 0)
            {
                return;
            }

            var qs = await easyToGetWrongQuestionsTask;
            foreach (var item in qs.Items)
            {
                VM.Items.Add(item);
            }
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is EasyToGetWrongQuestionModel model))
                return;

            await Navigation.PushAsync(new WrongAnswerDetailTabbedPage(model.QuestionId));
        }
    }
}