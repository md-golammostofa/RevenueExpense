using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ReportModels
{
    public class MonthlyShopChargesDetail
    {
        public long OrganizationId { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public string MonthAndYear { get; set; }
        public long FloorId { get; set; }
        public string FloorNo { get; set; }
        public long ShopId { get; set; }
        public string ShopName { get; set; }
        public long ChargeId { get; set; }
        public string FundDetailNameBN { get; set; }
        public decimal Amount { get; set; }
        public string AmountBN { get; set; }
    }
}