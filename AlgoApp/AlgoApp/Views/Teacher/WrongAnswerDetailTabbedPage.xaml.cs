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
    public partial class WrongAnswerDetailTabbedPage : TabbedPage
    {
        private readonly int questionId;

        public WrongAnswerDetailTabbedPage()
        {
            InitializeComponent();
        }

        public WrongAnswerDetailTabbedPage(int questionId)
        {
            this.questionId = questionId;
            Children.Add(new WrongAnswerUserListPage(questionId) { Title = "详细列表" });
            Children.Add(new QuestionPage(questionId) {Title = "题目详情" });
        }
    }
}