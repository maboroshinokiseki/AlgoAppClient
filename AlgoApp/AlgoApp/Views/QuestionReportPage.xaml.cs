using AlgoApp.Models.Data;
using AlgoApp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionReportPage : ContentPage
    {
        private int questionId;

        private QuestionReportPage()
        {
            InitializeComponent();
        }
        public QuestionReportPage(int questionId):this()
        {
            this.questionId = questionId;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var appServer = DependencyService.Get<IAppServer>();
            var message = new MessageModel { UserId = App.UserId, MessageType = MessageType.QuestionReport };
            var content = new { Content = messageEditor.Text, QuestionId = questionId };
            message.Content = JsonConvert.SerializeObject(content);
            await appServer.PostMessage(message);
        }
    }
}