using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    [Table("V_Suppliers")]
    public class Supplier
    {
        public string SupplierName { get; set; }
        public string Id { get; set; }
        public string Mail { get; set; }    
    }
}