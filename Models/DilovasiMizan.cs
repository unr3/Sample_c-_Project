using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    [Table("V_DilovasiMizan")]
    public class DilovasiMizan
    {
        [Key]
        public string STOK_KODU { get; set; }
        public string STOK_ADI { get; set; }
        public string INGISIM { get; set; }

        public string FIYAT3 { get; set; }
        public string BAKIYE { get; set; }
    }
}