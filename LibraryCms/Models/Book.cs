using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryCms.Models
{
    public class Book
    {
        public string BookId { get; set; }

        public string BookName { get; set; }

        public string Author { get; set; }

        public string Publisher { get; set; }

        public int Pages { get; set; }

        public string PublicTime { get; set; }

        public string Formart { get; set; }

        public string BookPath { get; set; }

        public int DownloadNumber { get; set; }

        public float Point { get; set; }

        public string DepartmentId { get; set; }

    }
}