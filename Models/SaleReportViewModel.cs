using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    public class SaleReportViewModel
    {
        public IEnumerable<ReferanceNumber> ReferanceNumbers { get; set; }
        public IEnumerable<VFile> Files { get; set; }

    }
}