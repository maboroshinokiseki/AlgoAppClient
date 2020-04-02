using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoApp.Models.Data
{
    class HistoryListModel : CommonResultModel
    {
        public List<HistoryItemModel> historyItems { get; set; }
    }
}
