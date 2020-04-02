using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoApp.Models.Data
{
    public class HistoryItemModel
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public string QuestionContent { get; set; }
        public bool Correct { get; set; }
    }
}
