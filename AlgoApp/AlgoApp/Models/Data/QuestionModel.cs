using AlgoApp.ViewModels;
using System.Collections.Generic;

namespace AlgoApp.Models.Data
{
    public class QuestionModel : CommonResultModel
    {
        public int Id { get; set; }
        public string ContentWithIndex { get; set; }
        public string Content { get; set; }
        public QuestionType Type { get; set; }
        public List<Option> Options { get; set; }
        public QuestionStatus Status { get; set; }
        public string Analysis { get; set; }
        public AnswerResultModel AnswerResult { get; set; }
        public class Option : BaseViewModel
        {
            private bool isChecked;

            public int Id { get; set; }
            public string Content { get; set; }
            public bool IsChecked
            {
                get => isChecked;
                set => SetValue(out isChecked, value);
            }
        }
    }
}
