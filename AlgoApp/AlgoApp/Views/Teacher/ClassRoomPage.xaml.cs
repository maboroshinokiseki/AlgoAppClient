﻿using AlgoApp.Models.Data;
using AlgoApp.Services;
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

        public Command RemoveStudentCommand { get; }
        public ObservableCollection<UserModel> Items { get; set; }

        public ClassRoomPage()
        {
            InitializeComponent();

            appServer = DependencyService.Get<IAppServer>();

            Items = new ObservableCollection<UserModel>();

            BindingContext = this;
        }

        public ClassRoomPage(int classId): this()
        {
            classRommTask = appServer.ClassRoom(classId);
            RemoveStudentCommand = new Command(async user =>
            {
                var u = user as UserModel;
                await appServer.RemoveStudentFromClass(u.Id, classId);
                Items.Remove(u);
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Items.Count != 0)
            {
                return;
            }

            MyListView.IsRefreshing = true;
            var classRoom = await classRommTask;

            foreach (var item in classRoom.Students)
            {
                Items.Add(item);
            }

            MyListView.IsRefreshing = false;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is UserModel user))
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
    }
}