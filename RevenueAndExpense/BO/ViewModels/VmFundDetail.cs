using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ViewModels
{
    public class VmFundDetail
    {
        public long FundDetailId { get; set; }
        [StringLength(200)]
        public string FundDetailName { get; set; }
        [StringLength(200),Required]
        public string FundDetailNameBN { get; set; }
        [StringLength(50), Required]
        public string Type { get; set; }
        [StringLength(50), Required]
        public string TypeBN { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public bool? IsActive { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        [Required]
        public long? FundInfoId { get; set; }
        [StringLength(100)]
        public string FundName { get; set; }
        public decimal AmountEN { get; set; }
        [StringLength(30)]
        public string AmountBN { get; set; }
        public bool IsFan { get; set; }
        public decimal? OpeningBalance { get; set; }
        public bool IsMonthlyChargeable { get; set; }
    }
}