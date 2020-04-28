﻿using System;

namespace AlgoApp.Models
{
    public class MasterMenuItemModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Type TargetType { get; set; }
        public Action Action { get; set; }
    }
}
