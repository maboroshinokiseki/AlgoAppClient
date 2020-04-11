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
    public partial class ProfilePage : ContentPage
    {
        private readonly IAppServer appServer;
        private readonly CommonListViewViewModel<Item> VM;
        private readonly Task<UserModel> userTask;
        private readonly bool self;

        private ProfilePage()
        {
            InitializeComponent();
        }

        public ProfilePage(int userId, bool self = false):this()
        {
            appServer = DependencyService.Get<IAppServer>();
            VM = new CommonListViewViewModel<Item> { Items = new ObservableCollection<Item>() };
            BindingContext = VM;
            userTask = appServer.GetUserDetail(userId);
            this.self = self;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (VM.Items.Count > 0)
            {
                return;
            }

            var user = await userTask;
            string gender = "";
            switch (user.Gender)
            {
                case Gender.Secrecy:
                    gender = "秘密";
                    break;
                case Gender.Male:
                    gender = "男";
                    break;
                case Gender.Female:
                    gender = "女";
                    break;
                default:
                    break;
            }
            if (self)
            {
                VM.Items.Add(new Item { Name = "用户名", Value = user.Username });
                VM.Items.Add(new Item { Name = "密码", Value = "******" });
            }
            VM.Items.Add(new Item { Name = "昵称", Value = user.Nickname });
            VM.Items.Add(new Item { Name = "性别", Value = gender });
            VM.Items.Add(new Item { Name = "生日", Value = user.BirthDay.ToString("yyyy-MM-dd") });
            VM.Items.Add(new Item { Name = "正确率", Value = user.CorrectRatio.ToString("P") });
            VM.Items.Add(new Item { Name = "做题数", Value = user.DoneQuestionCount.ToString() });
            VM.Items.Add(new Item { Name = "积分", Value = user.Points.ToString() });
        }

        class Item
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        //class ViewModel : BaseViewModel
        //{
        //    private string username;

        //    public string Username
        //    {
        //        get => username;
        //        set => SetValue(out username, value);
        //    }

        //    private string password;

        //    public string Password
        //    {
        //        get => password;
        //        set => SetValue(out password, value);
        //    }

        //    private string nickname;

        //    public string Nickname
        //    {
        //        get => nickname;
        //        set => SetValue(out nickname, value);
        //    }
        //}
    }
}