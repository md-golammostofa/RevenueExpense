using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.Models
{
    [Table("tblRevenueExpenses")]
    public class RevenueExpense
    {
        [Key]
        public long RevExId { get; set; }
        [StringLength(200)]
        public string RevExName { get; set; }
        public decimal Amount { get; set; }
        public long FundInfoId { get; set; }
        public long OrganizationId { get; set; }
        public Nullable<DateTime> BillDate { get; set; }
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
        public bool IsOpeningBalance { get; set; }

        [StringLength(30)]
        public string StateStatus { get; set; }
        [StringLength(30)]
        public string BillNo { get; set; }
        [StringLength(500)]
        public string PayFromOrPayTo { get; set; }

        [ForeignKey("FundDetail")]
        public long? FundDetailId { get; set; }
        public FundDetail FundDetail { get; set; }
    }
}