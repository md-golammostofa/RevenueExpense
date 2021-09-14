using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ViewModels
{
    public class VmDashboardFundDetail
    {
        public long FundInfoId { get; set; }
        public string FundNameBN { get; set; }
        public long FundDetailId { get; set; }
        public string FundDetailNameBN { get; set; }
        public decimal Amount { get; set; }
        public string AmountBN { get; set; }
    }
}