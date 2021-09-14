using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ViewModels
{
    public class VmDashBoardTodayFundRevenue
    {
        public long FundInfoId { get; set; }
        public string FundNameBN { get; set; }
        public decimal Amount { get; set; }
        public string AmountBN { get; set; }
    }

    public class VmDashBoardMonthlyBillDueCount
    {
        public long ShopId { get; set; }
        public string ShopName { get; set; }
        public int TotalDueMonth { get; set; }
    }

    public class VmDashBoardDueBillDetail
    {
        public string ShopName { get; set; }
        public string BillMonth { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal CurrentBill { get; set; }
        public string BillExpireDate { get; set; }
        public decimal Fine { get; set; }
        public decimal GrossAmount { get; set; }
        public string TotalAmountBN { get; set; }
        public string FineBN { get; set; }
        public string GrossAmountBN { get; set; }
    }

    public class VmDashBoardFundBalance
    {
        public long FundId { get; set; }
        public string FundNameBN { get; set; }
        public decimal CashAmount{ get; set; }
        public decimal BankAmount { get; set; }
        public string CashAmountBN { get; set; }
        public string BankAmountBN { get; set; }
    }

    public class VmDashBoardBankBalance
    {
        public long BankId { get; set; }
        public string Bank { get; set; }
        public long FundInfoId { get; set; }
        public string FundNameBN { get; set; }
        public decimal Balance { get; set; }
        public string BalanceBN { get; set; }
    }
}