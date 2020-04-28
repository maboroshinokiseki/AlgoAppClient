using AlgoApp.Models.Data;
using AlgoApp.Services;
using AlgoApp.ViewModels;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileEditPage : ContentPage
    {
        private BaseViewModel sideMaster;
        private UserModel model;
        private IAppServer appServer;

        private ViewModel VM { get; }

        private ProfileEditPage()
        {
            InitializeComponent();
        }
        public ProfileEditPage(UserModel model, BaseViewModel sideMaster) : this()
        {
            this.sideMaster = sideMaster;
            this.model = model;
            appServer = DependencyService.Get<IAppServer>();
            VM = new ViewModel { Password = model.Password, Birthday = model.BirthDay, SelectedGender = (int)model.Gender, Nickname = model.Nickname };
            VM.Genders = new ObservableCollection<string>
            {
                "保密",
                "男",
                "女"
            };
            VM.SaveUserInfoCommand = new Command(async () =>
            {
                model.BirthDay = VM.Birthday;
                model.Gender = (Gender)VM.SelectedGender;
                model.Nickname = VM.Nickname;
                model.Password = VM.Password;
                switch (model.Role)
                {
                    case UserRole.Teacher:
                        (sideMaster as Teacher.MainPageMaster.MainPageMasterViewModel).Name = VM.Nickname;
                        break;
                    case UserRole.Student:
                        (sideMaster as Student.MainPageMaster.MainPageMasterViewModel).Name = VM.Nickname;
                        break;
                    default:
                        break;
                }
                await appServer.UpdateUserInfo(model);
                await DisplayAlert("成功", "成功更新信息", "确认");
            });
            BindingContext = VM;
        }

        class ViewModel : BaseViewModel
        {
            private string password;
            private ObservableCollection<string> genders;
            private int selectedGender;
            private DateTime birthday;
            private string nickname;

            public string Password
            {
                get => password;
                set => SetValue(out password, value);
            }

            public string Nickname { get => nickname; set => SetValue(out nickname , value); }

            public ObservableCollection<string> Genders
            {
                get => genders;
                set => SetValue(out genders, value);
            }

            public int SelectedGender
            {
                get => selectedGender;
                set => SetValue(out selectedGender, value);
            }

            public DateTime Birthday
            {
                get => birthday;
                set => SetValue(out birthday, value);
            }

            public Command SaveUserInfoCommand { get; set; }
        }
    }
}