using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoApp.Models.Data
{
    class UserListModel : CommonResultModel
    {
        public List<UserModel> Users { get; set; }
    }
}
