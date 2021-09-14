using RevenueAndExpense.DAL.DataBaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BLL.Utility
{
    public class Checker
    {
        private ApplicationDbContext db;
        public Checker()
        {
            db = new ApplicationDbContext();
        }
        public static string ElementValue(string value)
        {
            string val = string.Empty;
            if (!string.IsNullOrEmpty(value.Trim()))
            {
                var outcome = Checker.GetMaleciousText().SingleOrDefault(txt => txt == value);
                if (string.IsNullOrEmpty(outcome))
                    val = value;
            }
            return val;
        }
        public static List<string> GetMaleciousText()
        {
            return new List<string>()
            {
                ";Drop",";Delete",";Truncate",";Alter",";Database",";Alter",";Add"
            };
        }
        public decimal GetFundBankBalance(int bankId, long fundInfoId, long orgId)
        {
            decimal balance = 0;
            if (bankId > 0 && fundInfoId > 0 && orgId > 0)
            {
                balance = db.Database.SqlQuery<decimal>(string.Format(@"Exec spGetFundBankBalance {0}, {1}, {2}", bankId, fundInfoId, orgId)).Single();
            }
            return balance;
        }
        public decimal GetFundCashBalance(long fundInfoId, long orgId)
        {
            decimal balance = 0;
            if (fundInfoId > 0 && orgId > 0)
            {
                balance = db.Database.SqlQuery<decimal>(string.Format(@"Exec spGetFundCashBalance {0},{1}", fundInfoId, orgId)).Single();
            }
            return balance;
        }
        public bool IsUserNameExist(string UserName, long Id) {
            return db.tblUsers.FirstOrDefault(u => u.UserName == UserName && u.UserId != Id) != null ? true : false;
        }
    }
}