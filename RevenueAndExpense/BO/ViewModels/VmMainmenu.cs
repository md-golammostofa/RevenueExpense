using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ViewModels
{
    public class VmMainmenu
    {
        public long MenuId { get; set; }
        [StringLength(100),Required]
        public string MenuName { get; set; }
        [StringLength(100)]
        public string IconClass { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
    }
}