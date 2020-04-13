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

namespace AlgoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RankingTabbedPage : TabbedPage
    {
        public RankingTabbedPage()
        {
            InitializeComponent();
            this.VM = new ViewModel();
            appServer = DependencyService.Get<IAppServer>();
            BindingContext = VM;
        }

        private ViewModel VM { get; }

        private readonly IAppServer appServer;

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (VM.YesterdayTop10 != null)
            {
                return;
            }

            VM.YesterdayTop10 = new ObservableCollection<UserModel>((await appServer.YesterdayTop10()).Items);
            VM.AllTimeTop10 = new ObservableCollection<UserModel>((await appServer.AllTimeTop10()).Items);
        }

        class ViewModel : BaseViewModel
        {
            private ObservableCollection<UserModel> yesterdayTop10;
            private ObservableCollection<UserModel> allTimeTop10;

            public ObservableCollection<UserModel> YesterdayTop10 { get => yesterdayTop10; set => SetValue(out yesterdayTop10, value); }
            public ObservableCollection<UserModel> AllTimeTop10 { get => allTimeTop10; set => SetValue(out allTimeTop10, value); }
        }
    }
}