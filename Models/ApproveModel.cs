using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Appa_MVC.Models;

namespace Inspinia_MVC5_SeedProject.Models
{
    public class ApproveModel
    {
        public IEnumerable<ReferanceNumber> ReferanceNumbers { get; set; }
        public IEnumerable<Supplier> Suppliers { get; set; }
    }
}