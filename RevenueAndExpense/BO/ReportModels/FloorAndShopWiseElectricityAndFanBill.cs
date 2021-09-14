using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ReportModels
{
    public class FloorAndShopWiseElectricityAndFanBillInfo
    {
        public long FloorId { get; set; }
        public string FloorNo { get; set; }
        public long ShopId { get; set; }
        public long FundDetailId { get; set; }
        public string ShopName { get; set; }
        public string FundDetailNameBN { get; set; }
        public decimal ConsumUnit { get; set; }
        public string ConsumUnitBN { get; set; }
        public decimal  UnitRate { get; set; }
        public string UnitRateBN { get; set; }
        public decimal Amount { get; set; }
        public string AmountBN { get; set; }
    }
    public class FloorAndShopWiseElectricityAndFanBillSummery
    {
        public decimal ElectricityConsumeUnit { get; set; }
        public string ElectricityConsumeUnitBN { get; set; }
        public decimal FanConsumeUnit { get; set; }
        public string FanConsumeUnitBN { get; set; }
        public decimal RoadLineConsumeUnit { get; set; }
        public string RoadLineConsumeUnitBN { get; set; }
        public decimal TotalCurrentConsumeUnit { get; set; }
        public string TotalCurrentConsumeUnitBN { get; set; }
        public decimal ElectricityAmount { get; set; }
        public string ElectricityAmountBN { get; set; }
        public decimal FanAmount { get; set; }
        public string FanAmountBN { get; set; }
        public decimal RoadLineAmount { get; set; }
        public string RoadLineAmountBN { get; set; }
        public decimal TotalAmount { get; set; }
        public string TotalAmountBN { get; set; }
    }
}