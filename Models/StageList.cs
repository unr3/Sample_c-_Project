using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    public class StageList
    {
        public enum Stages
        {
          Start,
          Writing,
          AddingSupplier,
          OnApproval,
          Pricing,
          OnSaleDepartment,
          OnSplitting,
          Complated
        }
    }
}