using AlgoApp.Models.Data;
using AlgoApp.Services;
using AlgoApp.ViewModels;
using Newtonsoft.Json;
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
    public partial class NewQuestionSuggestionPage : ContentPage
    {
        public NewQuestionSuggestionPage()
        {
            InitializeComponent();
            appServer = DependencyService.Get<IAppServer>();
            VM = new ViewModel();
            VM.Options = new ObservableCollection<Option>();
            VM.DeleteOptionCommand = new Command<Option>((o) =>
            {
                VM.Options.Remove(o);
                VM.OptionListHeight = VM.Options.Count * 50;
            });
            VM.AddOptionCommand = new Command(() =>
            {
                VM.Options.Add(new Option { IsCorrect = false });
                VM.OptionListHeight = VM.Options.Count * 50;
            });
            VM.PostMessageCommand = new Command(async () => await appServer.PostMessage(new MessageModel { UserId = App.UserId, MessageType = MessageType.NewQuestion, Content = JsonConvert.SerializeObject(VM) }));
            BindingContext = VM;
        }

        private readonly IAppServer appServer;

        private ViewModel VM { get; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var chapters = await appServer.GetChaperListAsync();
            VM.Chapters = new ObservableCollection<ChapterModel>(chapters.Items);
            VM.SelectedChapter = VM.Chapters[0];
        }

        class ViewModel : BaseViewModel
        {
            private ObservableCollection<ChapterModel> chapters;
            private ChapterModel selectedChapter;
            private int optionListHeight;

            public ObservableCollection<ChapterModel> Chapters { get => chapters; set => SetValue(out chapters, value); }
            public ChapterModel SelectedChapter { get => selectedChapter; set => SetValue(out selectedChapter, value); }
            public int OptionListHeight { get => optionListHeight; set => SetValue(out optionListHeight, value); }
            public string Content { get; set; }
            public ObservableCollection<Option> Options { get; set; }
            public Command<Option> DeleteOptionCommand { get; set; }
            public Command AddOptionCommand { get; set; }
            public Command PostMessageCommand { get; set; }
        }

        class Option : BaseViewModel
        {
            private bool isCorrect;

            public string OptionText { get; set; }
            public bool IsCorrect { get => isCorrect; set => SetValue(out isCorrect, value); }
        }
    }
}