using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlgoApp.Views.Teacher
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentDetailTabbedPage : TabbedPage
    {
        public StudentDetailTabbedPage()
        {
            InitializeComponent();
        }

        public StudentDetailTabbedPage(int id) : this()
        {
            this.Children.Add(new ProfilePage(id) { Title = "用户信息" });
            this.Children.Add(new ChapterListPage(ChapterListPage.PageType.AnswerHistory, id) { Title = "答题历史" });
        }
    }
}