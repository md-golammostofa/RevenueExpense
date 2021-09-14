using RevenueAndExpense.BO.ViewModels;
using RevenueAndExpense.DAL.DataBaseContext;
using RevenueAndExpense.DAL.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RevenueAndExpense.Controllers
{
    [CustomAuthorize]
    public class UserController : BaseController
    {
        // GET: User
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.DashBoardTodayFundRevenue = db.Database.SqlQuery<VmDashBoardTodayFundRevenue>(string.Format(@"Exec spDashBoardTodayFundRevenue {0}", User.OrgId)).ToList();

            ViewBag.DashBoardThisMonthFundRevenue = db.Database.SqlQuery<VmDashBoardTodayFundRevenue>(string.Format(@"Exec spDashBoardThisMonthFundRevenue {0}", User.OrgId)).ToList();

            ViewBag.DashboardMonthlyBillDueCount = db.Database.SqlQuery<VmDashBoardMonthlyBillDueCount>(string.Format(@"Exec spDashboardMonthlyBillDueCount {0}", User.OrgId)).ToList();

            ViewBag.DashBoardFundBalance = db.Database.SqlQuery<VmDashBoardFundBalance>(string.Format(@"Exec spFundBalance {0}", User.OrgId)).ToList();

            ViewBag.DashBoardBankBalance = db.Database.SqlQuery<VmDashBoardBankBalance>(string.Format(@"Exec spBankBalance {0}", User.OrgId)).ToList();

            return View();
        }
    }
}