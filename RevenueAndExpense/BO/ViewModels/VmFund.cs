using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ViewModels
{
    public class VmFund
    {
        public long FundInfoId { get; set; }
        [StringLength(100)]
        public string FundName { get; set; }
        [StringLength(100),Required]
        public string FundNameBN { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long? OrganizationId { get; set; }
        [StringLength(100)]
        public string  OrganizationName { get; set; }
    }
}