using AlgoApp.Models;
using AlgoApp.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views.Student
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
            Master = new MainPageMaster(this);
            Detail = new NavigationPage(new ChapterListPage());
        }
    }
}