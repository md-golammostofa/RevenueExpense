using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ViewModels
{
    public class VmFloor
    {
        public long FloorId { get; set; }
        [StringLength(100),Required]
        public string FloorNo { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        // Navigation Property
        public Nullable<long> OrganizationId { get; set; }
        public string OrganizationName { get; set; }
    }
}