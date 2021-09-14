using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ViewModels
{
    public class VmRevenueExpense
    {
        public long RevExId { get; set; }
        [StringLength(200),Required]
        public string RevExName { get; set; }
        public decimal Amount { get; set; }
        [Required, Range(1, long.MaxValue)]
        public long FundInfoId { get; set; }
        [Required,Range(1,long.MaxValue)]
        public long? FundDetailId { get; set; }
        public long OrganizationId { get; set; }
        [StringLength(50)]
        public string BillDate { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [StringLength(150)]
        public string Remark { get; set; }
        [StringLength(30)]
        public string AmountBN { get; set; }

        [StringLength(30)]
        public string StateStatus { get; set; }
        [StringLength(30)]
        public string BillNo { get; set; }
        [StringLength(500)]
        public string PayFromOrPayTo { get; set; }
    }

    public class VmRevenueExpenseDto
    {
        public long RevExId { get; set; }
        [StringLength(200)]
        public string RevExName { get; set; }
        public long FundInfoId { get; set; }
        [StringLength(100)]
        public string FundNameBN { get; set; }
        public long FundDetailId { get; set; }
        [StringLength(200)]
        public string FundDetailNameBN { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
        [StringLength(50)]
        public string TypeBN { get; set; }
        [StringLength(50)]
        public string BillDate { get; set; }
        public decimal Amount { get; set; }
        [StringLength(30)]
        public string AmountBN { get; set; }
        [StringLength(500)]
        public string PayFromOrPayTo { get; set; }
        [StringLength(150)]
        public string Remark { get; set; }
    }
}