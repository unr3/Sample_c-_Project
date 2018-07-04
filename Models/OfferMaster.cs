using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    public class OfferMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string OfferId { get; set; }
        public string SupplierId { get; set; }
        public int FileId { get; set; }
        public bool IsClosed { get; set; }
        public bool IsComplated { get; set; }
        public string NameOfPriceGiven { get; set; }
        public DateTime? ComplatedDate { get; set; }


    }
}