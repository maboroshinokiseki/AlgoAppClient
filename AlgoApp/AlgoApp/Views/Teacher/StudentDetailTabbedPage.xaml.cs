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
            this.Children.Add(new StudentProfilePage(id));
            this.Children.Add(new AnswerHistoryPage(id));
        }
    }
}