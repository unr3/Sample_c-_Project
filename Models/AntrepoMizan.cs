using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    [Table("V_AntrepoMizan")]
    public class AntrepoMizan
    {
        [Key]
        public string STOK_KODU { get; set; }
        public string STOK_ADI { get; set; }
        public decimal? FIYAT3 { get; set; }
        public decimal? BAKIYE { get; set; }

    }
}