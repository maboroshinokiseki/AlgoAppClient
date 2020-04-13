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
    public partial class ClassWrongAnswerDetailTabbedPage : TabbedPage
    {
        private readonly int classId;
        private readonly int questionId;

        private ClassWrongAnswerDetailTabbedPage()
        {
            InitializeComponent();
        }

        public ClassWrongAnswerDetailTabbedPage(int classId, int questionId) : this()
        {
            this.classId = classId;
            this.questionId = questionId;
            Children.Add(new ClassWrongAnswerListPage(classId, questionId) { Title = "详细列表" });
            Children.Add(new QuestionPage(questionId) { Title = "题目详情" });
        }
    }
}