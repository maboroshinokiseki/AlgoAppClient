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
    public partial class AnswerHistoryPage : ContentPage
    {
        private readonly IAppServer appServer;
        private readonly Task<HistoryListModel> historyTask;
        private readonly AnswerHistoryPageViewModel vm;

        public AnswerHistoryPage()
        {
            InitializeComponent();
        }

        public AnswerHistoryPage(int uid) : this()
        {
            appServer = DependencyService.Get<IAppServer>();
            historyTask = appServer.GetUserAnswerHistory(uid);

            vm = new AnswerHistoryPageViewModel
            {
                Items = new ObservableCollection<ListModel>()
            };

            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (vm.Items.Count != 0)
            {
                return;
            }

            foreach (var item in (await historyTask).historyItems)
            {
                var source = item.Correct ? ImageSource.FromFile("ic_action_check.png") : ImageSource.FromFile("ic_icon_wrong.png");
                vm.Items.Add(new ListModel { AnswerId = item.AnswerId, QuestionId = item.QuestionId, QuestionContent = item.QuestionContent, ImageSource = source });
            }
        }

        private async void MyListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is ListModel model))
                return;

            await Navigation.PushAsync(new AnswerHistoryDetailPage(model.QuestionId, model.AnswerId));
        }
    }

    public class AnswerHistoryPageViewModel : BaseViewModel
    {
        public ObservableCollection<ListModel> Items { get; set; }
    }

    public class ListModel
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public string QuestionContent { get; set; }
        public ImageSource ImageSource { get; set; }
    }
}