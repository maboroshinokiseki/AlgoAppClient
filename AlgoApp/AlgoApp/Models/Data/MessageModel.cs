using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoApp.Models.Data
{
    class MessageModel
    {
        public int Id { get; set; }
        public MessageType MessageType { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public bool Read { get; set; }
    }
}
