using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Appa_MVC.Models;

namespace Appa_MVC.Models
{
    [Table("V_SupplierLineItem")]
    public class ViewSupplierPriceLineItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int OfferMasterId { get; set; }
        public OfferMaster OfferMaster { get; set; }
        public int LineItemId { get; set; }
        public LineItem LineItem { get; set; }
        public decimal SupplierPrice { get; set; }
        public string Comment { get; set; }
 
        public string Currency { get; set; }
        public int Vat { get; set; }
        public bool IsClosed { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsOldPrice { get; set; }
  
        public DateTime? OldPriceDate { get; set; }
 
        public DateTime? UpdateDate { get; set; }
        public string SupplierName { get; set; }
        public decimal RealPrice { get; set; }
        public string TurkishDescription { get; set; }





    }
}