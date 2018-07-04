using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    public class SalesFileViewModel
    {
        public IEnumerable<ReferanceNumber> ReferanceNumbers { get; set; } 
        public File File { get; set; }
    }
}