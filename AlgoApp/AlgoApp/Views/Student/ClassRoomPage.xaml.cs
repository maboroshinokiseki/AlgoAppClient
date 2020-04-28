using AlgoApp.Models.Data;
using AlgoApp.Services;
using AlgoApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace AlgoApp.Views.Student
{
    public partial class ClassRoomPage : ContentPage
    {
        private readonly IAppServer appServer;
        private readonly int classId;
        private ClassRoomModel classRoom;
        private Action<object, EventArgs> lastSorting;

        public ViewModel VM { get; }

        public ClassRoomPage()
        {
            InitializeComponent();
        }

        public ClassRoomPage(int classId) : this()
        {
            appServer = DependencyService.Get<IAppServer>();

            VM = new ViewModel
            {
                RemoveStudentCommand = new Command<ListItemModel>(async user =>
                {
                    await appServer.RemoveStudentFromClass(user.Id, classId);
                    VM.Items.Remove(user);
                })
            };
            BindingContext = VM;
            this.classId = classId;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (VM.Items != null && VM.AddedStudent == false)
            {
                return;
            }

            VM.Items = new ObservableCollection<ListItemModel>();
            VM.IsBusy = true;
            classRoom = await appServer.ClassRoom(classId);

            foreach (var item in classRoom.Students)
            {
                VM.Items.Add(ObjectMapper.Map<ListItemModel>(item));
            }
            lastSorting?.Invoke(null, null);
            VM.IsBusy = false;
            VM.AddedStudent = false;
        }

        public class ViewModel : CommonListViewViewModel<ListItemModel>
        {
            public Command RemoveStudentCommand { get; set; }
            public bool AddedStudent { get; set; }
        }

        public class ListItemModel : UserModel
        {
            private string display;

            public string Display
            {
                get => display;
                set => SetValue(out display, value);
            }
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is ListItemModel user))
                return;

            await Navigation.PushAsync(new ProfilePage(user.Id) { Title = user.Nickname });
        }

        private void SortByNameAscToolbarItem_Clicked(object sender, EventArgs e)
        {
            lastSorting = SortByNameAscToolbarItem_Clicked;
            SortItems(VM.Items.OrderBy(m => m.Nickname), m => "");
        }

        private void SortByNameDesToolbarItem_Clicked(object sender, EventArgs e)
        {
            lastSorting = SortByNameDesToolbarItem_Clicked;
            SortItems(VM.Items.OrderByDescending(m => m.Nickname), m => "");
        }

        private void SortByCorrectRatioAscToolbarItem_Clicked(object sender, EventArgs e)
        {
            lastSorting = SortByCorrectRatioAscToolbarItem_Clicked;
            SortItems(VM.Items.OrderBy(m => m.CorrectRatio).ThenBy(m => m.Nickname), m => "正确率：" + m.CorrectRatio.ToString("P"));
        }

        private void SortByCorrectRatioDesToolbarItem_Clicked(object sender, EventArgs e)
        {
            lastSorting = SortByCorrectRatioDesToolbarItem_Clicked;
            SortItems(VM.Items.OrderByDescending(m => m.CorrectRatio).ThenBy(m => m.Nickname), m => "正确率：" + m.CorrectRatio.ToString("P"));
        }

        private void SortByDoneCountAscToolbarItem_Clicked(object sender, EventArgs e)
        {
            lastSorting = SortByDoneCountAscToolbarItem_Clicked;
            SortItems(VM.Items.OrderBy(m => m.DoneQuestionCount).ThenBy(m => m.Nickname), m => "做题数：" + m.DoneQuestionCount);
        }

        private void SortByDoneCountDesToolbarItem_Clicked(object sender, EventArgs e)
        {
            lastSorting = SortByDoneCountDesToolbarItem_Clicked;
            SortItems(VM.Items.OrderByDescending(m => m.DoneQuestionCount).ThenBy(m => m.Nickname), m => "做题数：" + m.DoneQuestionCount);
        }

        private void SortByPointsAscToolbarItem_Clicked(object sender, EventArgs e)
        {
            lastSorting = SortByPointsAscToolbarItem_Clicked;
            SortItems(VM.Items.OrderBy(m => m.Points).ThenBy(m => m.Nickname), m => "积分：" + m.Points);
        }

        private void SortByPointsDesToolbarItem_Clicked(object sender, EventArgs e)
        {
            lastSorting = SortByPointsDesToolbarItem_Clicked;
            SortItems(VM.Items.OrderByDescending(m => m.Points).ThenBy(m => m.Nickname), m => "积分：" + m.Points);
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
