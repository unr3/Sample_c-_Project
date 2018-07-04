using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Appa_MVC.Models;

namespace Appa_MVC.Models
{
    public class SupplierPriceLineItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int? OfferMasterId { get; set; }
        public OfferMaster OfferMaster { get; set; }
        public int LineItemId { get; set; }
        public LineItem LineItem { get; set; }
        [DisplayName("Fiyatınız")]
        public decimal SupplierPrice { get; set; }
        public string Comment { get; set; }
        [DisplayName("Döviz")]
        public string Currency { get; set; }
        [DisplayName("KDV")]
        public int Vat { get; set; }
        public bool IsClosed { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsOldPrice { get; set; }
        [DisplayName("Son Girilen Fiyat Tarihi")]
        public DateTime? OldPriceDate { get; set; }

        [DisplayName("Güncelleme Tarihi")]
        public DateTime? UpdateDate { get; set; } 
        [DisplayName("Türkçe Tanım")]
        public string TurkishDescription { get; set; }

        public bool Deleted { get; set; }


    }
}