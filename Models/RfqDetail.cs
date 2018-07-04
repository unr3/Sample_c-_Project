using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    public class RfqDetail
    {
        public int RfqId { get; set; }
        public string RfqName { get; set; }
        public int Count { get; set; }
        public string Stage { get; set; }
        public byte StageId { get; set; }
        public string PoNumber { get; set; }


    }
}