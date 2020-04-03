using System.Collections.Generic;

namespace AlgoApp.Models.Data
{
    public class CommonListResultModel<T> : CommonResultModel
    {
        public List<T> Items { get; set; }
    }
}
