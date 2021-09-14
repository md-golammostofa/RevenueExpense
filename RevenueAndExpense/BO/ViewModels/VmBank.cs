using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ViewModels
{
    public class VmBank
    {
        public long BankId { get; set; }
        [StringLength(150),Required]
        public string BankName { get; set; }
        [StringLength(200), Required]
        public string BranchName { get; set; }
        [StringLength(100), Required]
        public string AccountNo { get; set; }
        public bool IsActive { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public double Amount { get; set; }
    }

    public class VmCashToBankAndBankToCash
    {
        public long Id { get; set; }
        public double? BankToCash { get; set; }
        public double? CashToBank { get; set; }
        [StringLength(100)]
        public string Remark { get; set; }
        [StringLength(50)]
        public string BankDate { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        [Required,Range(1,Int64.MaxValue)]
        public long BankId { get; set; }
        [StringLength(150)]
        public string BankName { get; set; }
        [Required, Range(1, Int64.MaxValue)]
        public long FundInfoId { get; set; }
        [StringLength(100)]
        public string FundNameBN { get; set; }
        public long OrganizationId { get; set; }
        [StringLength(100)]
        public string AccountNo { get; set; }
        [StringLength(50)]
        public string ChequeNo { get; set; }
        public bool IsOpeningBalance { get; set; }
    }

    public class VmFundBankOB
    {
        public long Id { get; set; }
        [Required, Range(1,long.MaxValue)]
        public long BankId { get; set; }
        public string BankName { get; set; }
        [Required, Range(1, long.MaxValue)]
        public long FundInfoId { get; set; }
        public string FundNameBN { get; set; }
        [Required,Range(1,double.MaxValue)]
        public double Amount { get; set; }
        public string AmountBN { get; set; }
        [Required]
        public string BankDateEN { get; set; }
        public string BankDateBN { get; set; }
    }

}