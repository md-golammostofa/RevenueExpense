using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ReportModels
{
    public class FloorAndShopWiseDueDetail
    {
        public long FloorId { get; set; }
        public string FloorNo { get; set; }
        public long ShopId { get; set; }
        public string ShopName { get; set; }
        public int TotalDueMonth { get; set; }
        public decimal CurrentBill { get; set; }
        public decimal OtherCharges { get; set; }
        public decimal Amount { get; set; }
        public decimal Fine { get; set; }
        public decimal ReconFee { get; set; }
        public decimal GrossAmount { get; set; }
        public string TotalDueMonthBN { get; set; }
        public string CurrentBillBN { get; set; }
        public string OtherChargesBN { get; set; }
        public string AmountBN { get; set; }
        public string FineBN { get; set; }
        public string ReconFeeBN { get; set; }
        public string GrossAmountBN { get; set; }
    }
    public class FloorAndShopWiseDueSummery
    {
        // Total//
        public decimal TotalCurrentBill { get; set; }
        public decimal TotalOtherCharges { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalFine { get; set; }
        public decimal TotalReconFee { get; set; }
        public decimal TotalGrossAmount { get; set; }
        //BN
        public string TotalCurrentBillBN { get; set; }
        public string TotalOtherChargesBN { get; set; }
        public string TotalAmountBN { get; set; }
        public string TotalFineBN { get; set; }
        public string TotalReconFeeBN { get; set; }
        public string TotalGrossAmountBN { get; set; }
        // 
        public decimal TotalDueAmount { get; set; }
        public string TotalDueAmountBN { get; set; }
        public string InWordBN { get; set; }
    }
}