using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoApp.Models.Data
{
    public class ClassRoomModel : CommonResultModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StudentCount { get; set; }
        public UserModel Teacher { get; set; }
        public List<UserModel> Students { get; set; }
    }
}
