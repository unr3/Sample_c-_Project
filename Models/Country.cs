using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace Appa_MVC.Models
{
    [Table("V_Country")]
    public class Country
    {
        public string ID { get; set; }
        public string ULKEADI { get; set; }

    }
}