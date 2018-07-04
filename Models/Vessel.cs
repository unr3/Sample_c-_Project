using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    [Table("V_Vessels")]
    public class Vessel
    {
        public string VesselName { get; set; }
        public string Id { get; set; }

    }
}