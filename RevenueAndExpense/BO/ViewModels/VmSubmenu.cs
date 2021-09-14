using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ViewModels
{
    public class VmSubmenu
    {
        [Required]
        public long MenuId { get; set; }
        public string MenuName { get; set; }
        public long SubmenuId { get; set; }
        [Required,StringLength(100)]
        public string SubmenuName { get; set; }
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [Required,StringLength(150)]
        public string ControllerName { get; set; }
        [Required, StringLength(150)]
        public string ActionName { get; set; }
        public bool IsViewable { get; set; }
    }
}