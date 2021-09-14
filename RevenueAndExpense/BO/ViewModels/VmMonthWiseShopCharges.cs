using RevenueAndExpense.BLL.Validator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ViewModels
{
    public class VmShopMonthlyChargeInfo {
        public int? FloorId { get; set; }
        [Required(ErrorMessage = "শপ নং আবশ্যক")]
        public long ShopId { get; set; }
        [Required(ErrorMessage = "মাস ও সাল নাম আবশ্যক")]
        public string Date { get; set; }
        public ICollection<VmShopMonthlyChargeDetails> ChargeDetails { get; set; }
    }
    public class VmShopMonthlyChargeDetails
    {
        public long? ShopId { get; set; }
        public string ChargeMonth { get; set; }
        public long ChargeId { get; set; }
        public long? FundInfoId { get; set; }
        public string FundNameBN { get; set; }
        [Required]
        public long? FundDetailId { get; set; }
        [StringLength(200)]
        public string ChargeName { get; set; }
        //[CustomChargeRequireAttribute("Remark")]
        public decimal? PreviousReading { get; set; }
        //[CustomChargeRequireAttribute("Remark")]
        public decimal? CurrentReading { get; set; }
        //[CustomChargeRequireAttribute("Remark")]
        public decimal? ConsumUnit { get; set; }
        //[CustomChargeRequireAttribute("Remark")]
        public decimal? UnitRate { get; set; }
        //[CustomChargeRequireAttribute("Remark")]
        public decimal? Amount { get; set; }
        [Required]
        public string Remark { get; set; }
        public string StateStatus { get; set; }
    }
    public class VmShopMonthlyBill
    {
        public long ShopId { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        public long FloorId { get; set; }
        public string FloorNo { get; set; }
        public string ShopName { get; set; }
        public string MobileNo { get; set; }
        public string HomeAddress { get; set; }
        public string Email { get; set; }
        public string AmountBN { get; set; }
        public decimal Amount { get; set; }
        public string MonthAndYearBN { get; set; }
        public string StateStatus { get; set; }
        public string StateStatusBN { get; set; }
        public string Holding { get; set; }
    }
    public class VmBillStateStatus
    {
        public long shopId { get; set; }
        public short month { get; set; }
        public short year { get; set; }
    }
    public class VmChargeBillingView
    {
        public long ShopId { get; set; }
        public string ShopName { get; set; }
        public long FundInfoId { get; set; }
        public string FundNameBN { get; set; }
        public long FundDetailId { get; set; }
        public string FundDetailNameBN { get; set; }
        public decimal ChargeAmountEN { get; set; }
        public bool IsFan { get; set; }
    }
}