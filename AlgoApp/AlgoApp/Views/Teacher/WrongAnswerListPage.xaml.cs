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
        private readonly ViewModel VM;
        private readonly Task<CommonListResultModel<EasyToGetWrongQuestionModel>> easyToGetWrongQuestionsTask;

        public WrongAnswerListPage()
        {
            InitializeComponent();
        }

        public WrongAnswerListPage(int classId) : this()
        {
            appServer = DependencyService.Get<IAppServer>();
            VM = new ViewModel { Items = new ObservableCollection<EasyToGetWrongQuestionModel>() };
            easyToGetWrongQuestionsTask = appServer.GetEasyToGetWrongQuestions(classId);
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
    }

    public class ViewModel : BaseViewModel
    {
        private ObservableCollection<EasyToGetWrongQuestionModel> items;
        public ObservableCollection<EasyToGetWrongQuestionModel> Items
        {
            get => items;
            set => SetValue(out items, value);
        }
    }
}