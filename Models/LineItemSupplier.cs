using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    public class LineItemSupplier
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public int LineItemId { get; set; }
        public LineItem LineItem { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public bool ImpaChanged { get; set; } 


    }
}