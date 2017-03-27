using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryCms.Models
{
    public class Message
    {
        public int MessageID { get; set; }
        public string From { get; set; }
        public DateTime Time { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
    }
}