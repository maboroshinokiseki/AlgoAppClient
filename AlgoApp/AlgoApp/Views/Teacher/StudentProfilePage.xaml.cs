using AlgoApp.Models.Data;
using AlgoApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views.Teacher
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentProfilePage : ContentPage
    {
        private readonly IAppServer appServer;
        private readonly Task<UserModel> userTask;

        public StudentProfilePage()
        {
            InitializeComponent();
        }

        public StudentProfilePage(int id) : this()
        {
            appServer = DependencyService.Get<IAppServer>();
            userTask = appServer.GetUserDetail(id);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var user = await userTask;

            nameLabel.Text = user.NickName;
            correctRatioLabel.Text = (user.CorrectRatio * 100).ToString() + "%";
            doneQuestionCountLabel.Text = user.DoneQuestionCount.ToString();
            pointsLabel.Text = user.Points.ToString();
            birthDayLabel.Text = user.BirthDay.ToString("yyyy-MM-dd");
            switch (user.Gender)
            {
                case Gender.Secrecy:
                    genderLabel.Text = "秘密";
                    break;
                case Gender.Male:
                    genderLabel.Text = "男";
                    break;
                case Gender.Female:
                    genderLabel.Text = "女";
                    break;
                default:
                    break;
            }
        }
    }
}