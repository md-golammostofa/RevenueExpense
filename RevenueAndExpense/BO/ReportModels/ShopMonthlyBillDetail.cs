using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ReportModels
{
    public class ShopMonthlyBillDetail
    {
        public string SLNo { get; set; }
        public long FundDetailId { get; set; }
        public long ChargeId { get; set; }
        public long ShopId { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        public string ChargeName { get; set; }
        public string Currency { get; set; }
        public string AmountBN { get; set; }
        public decimal Amount { get; set; }
    }
}