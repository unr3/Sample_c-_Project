using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    public class FileMasterDetailsViewModel
    {
        public string Vessel { get; set; }
        public string File { get; set; }
        public string Rfq { get; set; }
        public string Stage { get; set; }
        public int RFQId { get; set; }

    }
}