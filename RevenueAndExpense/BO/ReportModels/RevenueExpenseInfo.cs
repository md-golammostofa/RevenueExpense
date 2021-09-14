using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ReportModels
{
    public class RevenueExpenseDetail
    {
        public string FIName { get; set; }
        public string RevRecDate { get; set; }
        public string RevenueName { get; set; }
        public decimal RevAmount { get; set; }
        public string RevAmountBN { get; set; }
        public string ExpenseName { get; set; }
        public string ExRecDate { get; set; }
        public decimal ExpAmount { get; set; }
        public string ExpAmountBN { get; set; }
        public decimal TotalRevenue { get; set; }
        public string TotalRevenueBN { get; set; }
        public decimal TotalExpense { get; set; }
        public string TotalExpenseBN { get; set; }
        // New
        public decimal CashToBank { get; set; }
        public decimal BankToCash { get; set; }
        public string CashToBankBN { get; set; }
        public string BankToCashBN { get; set; }
        public decimal NetRevenue { get; set; }
        public decimal NetExpense { get; set; }
        public string NetRevenueBN { get; set; }
        public string NetExpenseBN { get; set; }
    }

    public class RevenueExpenseInfo
    {
        public string FundInfoNameBN { get; set; }
        public string FromDateBN { get; set; }
        public string ToDateBN { get; set; }
        public string Organization { get; set; }
    }
}
