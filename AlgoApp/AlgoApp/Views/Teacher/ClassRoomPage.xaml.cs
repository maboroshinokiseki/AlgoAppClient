using AlgoApp.Models.Data;
using AlgoApp.Services;
using AlgoApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AlgoApp.Views.Teacher
{
    public partial class ClassRoomPage : ContentPage
    {
        private readonly IAppServer appServer;
        private readonly Task<ClassRoomModel> classRommTask;
        private readonly int classId;

        public ViewModel VM { get; }

        public ClassRoomPage()
        {
            InitializeComponent();
        }

        public ClassRoomPage(int classId) : this()
        {
            appServer = DependencyService.Get<IAppServer>();

            classRommTask = appServer.ClassRoom(classId);

            VM = new ViewModel
            {
                Items = new ObservableCollection<ListItemModel>(),

                RemoveStudentCommand = new Command(async user =>
                {
                    var u = user as ListItemModel;
                    await appServer.RemoveStudentFromClass(u.Id, classId);
                    VM.Items.Remove(u);
                })
            };

            BindingContext = VM;
            this.classId = classId;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (VM.Items.Count != 0)
            {
                return;
            }

            MyListView.IsRefreshing = true;
            var classRoom = await classRommTask;

            foreach (var item in classRoom.Students)
            {
                VM.Items.Add(ObjectMapper.Map<ListItemModel>(item));
            }

            MyListView.IsRefreshing = false;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            VM.Items.Clear();
        }

        public class ViewModel : BaseViewModel
        {
            private ObservableCollection<ListItemModel> items;
            public ObservableCollection<ListItemModel> Items
            {
                get => items;
                set => SetValue(out items, value);
            }
            public Command RemoveStudentCommand { get; set; }
        }

        public class ListItemModel : UserModel
        {
            public string Display { get; set; }
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is ListItemModel user))
                return;

            await Navigation.PushAsync(new StudentDetailTabbedPage(user.Id) { Title = user.NickName });
        }

        private async void DeletionToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            var classRoom = await classRommTask;
            await appServer.DeleteClassRomm(classRoom.Id);
            await Navigation.PopAsync();
        }

        private async void RenameToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            var classRoom = await classRommTask;
            string name;
            while (true)
            {
                name = await DisplayPromptAsync("班級名字", "", initialValue: classRoom.Name);
                if (string.IsNullOrWhiteSpace(name))
                {
                    if (name != null)
                    {
                        await DisplayAlert("錯誤", "請輸入名字", "確認");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    break;
                }
            }

            await appServer.RenameClassRomm(classRoom.Id, name);
        }

        private async void AddStudentToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            var classRoom = await classRommTask;
            await Navigation.PushAsync(new AddStudentToClassPage(classRoom.Id));
        }

        private async void ShowEasyToGetWrongQuestionsToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WrongAnswerListPage(classId));
        }

        private void SortByNameAscToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            SortItems(VM.Items.OrderBy(m => m.NickName), m => "");
        }

        private void SortByNameDesToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            SortItems(VM.Items.OrderByDescending(m => m.NickName), m => "");
        }

        private void SortByCorrectRatioAscToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            SortItems(VM.Items.OrderBy(m => m.CorrectRatio).ThenBy(m => m.NickName), m => "正确率：" + m.CorrectRatio * 100 + "%");
        }

        private void SortByCorrectRatioDesToolbarItem_Clicked(object sender, EventArgs e)
        {
            SortItems(VM.Items.OrderByDescending(m => m.CorrectRatio).ThenBy(m => m.NickName), m => "正确率：" + m.CorrectRatio * 100 + "%");
        }

        private void SortByDoneCountAscToolbarItem_Clicked(object sender, EventArgs e)
        {
            SortItems(VM.Items.OrderBy(m => m.DoneQuestionCount).ThenBy(m => m.NickName), m => "做题数：" + m.DoneQuestionCount);
        }

        private void SortByDoneCountDesToolbarItem_Clicked(object sender, EventArgs e)
        {
            SortItems(VM.Items.OrderByDescending(m => m.DoneQuestionCount).ThenBy(m => m.NickName), m => "做题数：" + m.DoneQuestionCount);
        }

        private void SortByPointsAscToolbarItem_Clicked(object sender, EventArgs e)
        {
            SortItems(VM.Items.OrderBy(m => m.Points).ThenBy(m => m.NickName), m => "积分：" + m.Points);
        }

        private void SortByPointsDesToolbarItem_Clicked(object sender, EventArgs e)
        {
            SortItems(VM.Items.OrderByDescending(m => m.Points).ThenBy(m => m.NickName), m => "积分：" + m.Points);
        }

        private void SortItems(IEnumerable<ListItemModel> newItems, Func<ListItemModel, string> displayFunc)
        {
            var newitems = new ObservableCollection<ListItemModel>(newItems);
            foreach (var item in newitems)
            {
                item.Display = displayFunc.Invoke(item);
            }

            VM.Items = newitems;
        }
    }
}
