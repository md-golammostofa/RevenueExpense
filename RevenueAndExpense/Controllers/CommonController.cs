using RevenueAndExpense.BLL.Filters;
using RevenueAndExpense.BLL.Utility;
using RevenueAndExpense.BO.DataObject;
using RevenueAndExpense.DAL.DataBaseContext;
using RevenueAndExpense.DAL.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RevenueAndExpense.BO.ViewModels;
using RevenueAndExpense.BO.Models;

namespace RevenueAndExpense.Controllers
{
    [CustomAuthorize]
    public class CommonController : BaseController
    {
        private ApplicationDbContext db;
        Checker Checker;
        public CommonController()
        {
            db = new ApplicationDbContext();
            Checker = new Checker();
        }
        public ActionResult SideNavBar()
        {
            VmNavBar navBar = new VmNavBar();
            try
            {
                navBar = (VmNavBar)Session["UserAuth"];
            }

            catch (Exception ex)
            {

            }
            return PartialView("SideNavBar", navBar);
        }
        //[HttpGet]
        public ActionResult Topnavbar()
        {
            return PartialView("_Topnavbar");
        }

        #region Dropdown Action Method
        [HttpPost]
        public ActionResult GetFloorsForDDL()
        {
            var data = db.tblFloors.Where(f => f.OrganizationId == User.OrgId).Select(f => new Dropdown() { value = f.FloorId.ToString(), text = f.FloorNo }).ToList();
            return Json(data);
        }
        [HttpPost]
        public ActionResult GetFundInfoForDDL()
        {
            var data = db.tblFundInfo.Where(f => f.OrganizationId == User.OrgId).Select(f => new Dropdown { value = f.FundInfoId.ToString(), text = f.FundNameBN }).ToList();
            return Json(data);
        }
        [HttpPost]
        public ActionResult GetOrganizationForDDL()
        {
            var data = db.tblOrganizations.Where(org => (User.OrgName == "PlayOn24") || (org.OrganizationId == User.OrgId)).Select(org => new Dropdown
            {
                value = org.OrganizationId.ToString(),
                text = org.OrganizationName
            }).ToList();
            return Json(data);
        }
        [HttpPost]
        public ActionResult GetRoleForDDL()
        {
            var data = db.tblRoles.Where(role => (User.OrgName == "PlayOn24") || (role.RoleName != "System Admin" && role.RoleName != "System User")).Select(role => new Dropdown
            {
                value = role.RoleId.ToString(),
                text = role.RoleName
            }).ToList();
            return Json(data);
        }

        [HttpPost]
        public ActionResult GetFundDetailsForDDL(long FundInfoId)
        {
            var data = db.tblFundDetail.Where(fd => fd.FundInfoId == FundInfoId && fd.OrganizationId == User.OrgId && !fd.IsMonthlyChargeable).Select(
                    fd => new Dropdown
                    {
                        text = fd.FundDetailNameBN + " (" + fd.TypeBN + ")",
                        value = fd.FundDetailId.ToString()
                    }
                ).ToList();
            return Json(data);
        }

        [HttpPost]
        public ActionResult GetFundDetailsMonthlyChargeForDDL(long FundInfoId)
        {
            var data = db.tblFundDetail.Where(fd => fd.FundInfoId == FundInfoId && fd.OrganizationId == User.OrgId && fd.IsMonthlyChargeable).Select(
                    fd => new Dropdown
                    {
                        text = fd.FundDetailNameBN + " (" + fd.TypeBN + ")",
                        value = fd.FundDetailId.ToString()
                    }
                ).ToList();
            return Json(data);
        }

        [HttpPost]
        public ActionResult GetBankWithAcc()
        {
            var data = db.tblBanks.Where(b => b.OrganizationId == User.OrgId).Select(b => new Dropdown
            {
                text = b.BankName +" ("+b.AccountNo+")",
                value = b.BankId.ToString()
            }).ToList();
            return Json(data);
        }

        [HttpPost]
        public ActionResult GetHoldingsByFloorIdForDDL(long FloorId)
        {
            var data = db.tblHoldings.Where(h => h.OrganizationId == User.OrgId && h.FloorId == FloorId).Select(h=> new Dropdown {
                text = h.HoldingName,
                value = h.HoldingId.ToString()
            }).ToList();
            return Json(data);
        }

        [HttpPost]
        public ActionResult GetYearAndMonthByHoldingId(long holding)
        {
            var data = db.Database.SqlQuery<Dropdown>(string.Format(@"Select [text],[value] From (Select DISTINCT m.[Month], m.[Year],(dbo.fnGetMonthNameBN(m.[Month])+','+dbo.fnGetBnNumerical(cast(m.[year] as nvarchar(4)))) 'text',(Cast(m.ShopId as Nvarchar(10))+':'+dbo.fnGetLastDateOfAMonthEN(m.[Month],m.[Year])) 'value' From tblMonthWiseShopCharges m Where ShopId =
(Select Top 1 s.ShopId From tblShops s
Inner Join tblShopHolding sh on s.ShopId = sh.ShopId
Inner Join tblHoldings h on sh.HoldingId = h.HoldingId where  s.OrganizationId={0} and h.HoldingId ={1}) and m.StateStatus='Pending') t
Order By [Year],[Month] asc", User.OrgId, holding)).ToList();
            return Json(data);
        }

        [HttpPost]
        public ActionResult GetShopsByFloorIdForDDL(long floorId)
        {
            var data = db.Database.SqlQuery<Dropdown>(string.Format(@"Select Cast(s.ShopId as nvarchar(50)) 'value', (s.ShopName + ' ('+f.FloorNo+'/'+dbo.fnGetShopHoldings(s.ShopId,f.FloorId,s.OrganizationId)+')' ) 'text' From tblShops s
Inner Join tblFloors f on s.FloorId = f.FloorId
Where f.FloorId={0} and f.OrganizationId={1}
",floorId,User.OrgId)).ToList();
            return Json(data);
        }

        [HttpPost]
        public ActionResult GetNextMonthsForDDL()
        {
           var dropdown = Utility.GetMonthAndYearListNextBN(1).Select(mon=> new Dropdown {text=mon.Text,value=mon.Value });
            return Json(dropdown);
        }

        #endregion

        #region Boolean Checker
        [HttpPost]
        public ActionResult IsHoldingExistByFloorId(int holdingId, string holdingName, int floorId)
        {
            bool isExist = false;
            try
            {
                var data = db.tblHoldings.FirstOrDefault(h => h.HoldingName == holdingName && h.FloorId == floorId && h.HoldingId != holdingId);
                if (data != null)
                {
                    isExist = true;
                }
            }
            catch (Exception ex)
            {
            }
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsFundInfoBNExistByFundInfoId(int fundId, string fundNameBN)
        {
            bool IsExist = false;
            var data = db.tblFundInfo.Where(f => f.FundNameBN == fundNameBN && f.OrganizationId == User.OrgId && f.FundInfoId != fundId).FirstOrDefault();
            if (data != null)
            {
                IsExist = true;
            }
            return Json(IsExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsFundInfoENExistByFundInfoId(int fundId, string fundNameEN)
        {
            bool IsExist = false;
            var data = db.tblFundInfo.Where(f => f.FundName.ToLower() == fundNameEN.ToLower() && f.OrganizationId == User.OrgId && f.FundInfoId != fundId).FirstOrDefault();
            if (data != null)
            {
                IsExist = true;
            }
            return Json(IsExist);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsFundDetailBNExistByFundDetailId(int fundInfoId,int fundDelId, string fundDetailBN)
        {
            bool IsExist = false;
            var data = db.tblFundDetail.Where(f => f.FundInfoId == fundInfoId && f.FundDetailNameBN == fundDetailBN && f.OrganizationId == User.OrgId && f.FundDetailId != fundDelId).FirstOrDefault();
            if (data != null)
            {
                IsExist = true;
            }
            return Json(IsExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsFundDetailENExistByFundDetailId(int fundInfoId, int fundDelId, string fundDetailEN)
        {
            bool IsExist = false;
            var data = db.tblFundDetail.Where(f => f.FundInfoId == fundInfoId && f.FundDetailName == fundDetailEN && f.OrganizationId == User.OrgId && f.FundDetailId != fundDelId).FirstOrDefault();
            if (data != null)
            {
                IsExist = true;
            }
            return Json(IsExist);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsUserNameExist(string UserName, long Id)
        {
            return Json(Checker.IsUserNameExist(UserName, Id));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsMainmenuExist(long id, string mainmenu)
        {
            bool isExist = true;
            var data = db.tblMainmenus.FirstOrDefault(m => m.MenuId != id && m.MenuName == mainmenu);
            if (data == null)
            {
                isExist = false;
            }
            return Json(isExist);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsFloorExist(long id, string floorName)
        {
            bool IsExist = false;
            var data = db.tblFloors.FirstOrDefault(f => f.FloorId != id && f.FloorNo == floorName && f.OrganizationId == User.OrgId);
            if (data != null)
            {
                IsExist = true;
            }
            return Json(IsExist);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsBankExist(long id,string bankName)
        {
            bool IsExist = false;
            var data = db.tblBanks.FirstOrDefault(b => b.OrganizationId == User.OrgId && bankName == b.BankName && b.BankId != id);
            if (data != null) {
                IsExist = true;
            }
            return Json(IsExist);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsBankAccExist(long id, string bankAcc)
        {
            bool IsExist = false;
            var data = db.tblBanks.FirstOrDefault(b => b.OrganizationId == User.OrgId && bankAcc == b.AccountNo && b.BankId != id);
            if (data != null)
            {
                IsExist = true;
            }
            return Json(IsExist);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsChequeNoExist(long bankId, long fundInfoId,string chequeNo)
        {
            bool IsExist = false;
            var data = db.tblCashToBankAndBankToCashes.FirstOrDefault(cb => cb.BankId == bankId && cb.FundInfoId == fundInfoId && cb.ChequeNo == chequeNo);
            if (data != null)
            {
                IsExist = true;
            }
            return Json(IsExist);
        }
        #endregion

        #region AutoComplete
        [HttpPost]
        public ActionResult GetHoldingNo(int floorId, string contextKey)
        {
            List<Dropdown> data = new List<Dropdown>();
            try
            {
                data = db.Database.SqlQuery<Dropdown>(string.Format(@"Select cast(h.HoldingId as nvarchar(25)) [value],h.HoldingName [text] From tblHoldings h
Inner Join tblFloors f on h.FloorId = f.FloorId and h.HoldingName like '%{1}%'
where f.FloorId = {0} and f.OrganizationId = {2} and h.HoldingId not in (select HoldingId from tblShopHolding sh where sh.OrganizationId = {2})", floorId, Checker.ElementValue(contextKey), User.OrgId)).ToList();

                if (data.Count == 0)
                {
                    Dropdown dropdown = new Dropdown() { value = "0", text = "কোন হোল্ডিং আর অবশিষ্ট নেই" };
                    data.Add(dropdown);
                }
            }
            catch (Exception ex)
            {

            }
            return Json(data);
        }
        [HttpPost]
        public ActionResult GetFundDetailsWithTypeByFundInfoId(int fundId, string contextKey)
        {
            var data = db.tblFundDetail.Where(fd => fd.OrganizationId == User.OrgId && fd.FundInfo.FundInfoId == fundId && (string.IsNullOrEmpty(contextKey) || fd.FundDetailNameBN.Contains(contextKey.Trim())) && fd.IsMonthlyChargeable).Select(fd => new AutoCompleteObj { value = fd.FundDetailId.ToString(), text = fd.FundDetailNameBN, amount = fd.AmountEN.ToString() }).ToList();
            if (data.Count == 0)
            {
                var obj = new AutoCompleteObj { value = "0", text = "কোন ডাটা পাওয়া যাইনি", amount = "0.00" };
                data.Add(obj);
            }
            return Json(data);
        }

        [HttpPost]
        public ActionResult GetShopNameWithFloorAndHolding(string contextKey)
        {
            List<Dropdown> list = new List<Dropdown>();
            try
            {
                string query = string.Format(@"Exec spShopList N'{0}',{1}", Checker.ElementValue(contextKey), User.OrgId);
                list = db.Database.SqlQuery<Dropdown>(query).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return Json(list);
        }
        [HttpPost]
        public ActionResult GetHoldingByFloorId(int floorId, string contextKey)
        {
            var data = db.tblHoldings.Include("tblFloors").Where(h => (floorId == 0 || h.FloorId == floorId) && (contextKey == "" || h.HoldingName == contextKey)).Select(s => new Dropdown
            {
                text = ("(ফ্লোরঃ" + s.Floor.FloorNo + ") -" + s.HoldingName),
                value = s.HoldingId.ToString()
            }).ToList();

            if (data == null)
            {
                data.Add(new Dropdown
                {
                    text = "কোন হোল্ডিং পাওয়া যায়নি",
                    value = ""
                });
            }

            return Json(data);
        }
        [HttpPost]
        public ActionResult GetShopListByFloorOrHoldingOrBoth(int floorId, int holdingId)
        {
            List<Dropdown> dropdowns = new List<Dropdown>();
            try
            {
                var floor = floorId > 0 ? string.Format(@" and f.FloorId={0}", floorId) : "";
                var holding = holdingId > 0 ? string.Format(@" and h.HoldingId={0}", holdingId) : "";
                var param = floor + holding;

                string query = string.Format(@"Select Distinct Cast(ShopId as Nvarchar(100)) 'value',(ShopName +N'(ফ্লোর:'+FloorNo+'- '+N'হোল্ডিং-'+SUBSTRING(HoldingNo,0,Len(HoldingNo))+')') as 'text'
From (Select s.ShopId,s.ShopName,s.ProprietorName,s.MobileNo,s.FloorId,f.FloorNo,s.StateStatus,s.EntryUser,
Cast((Select h.HoldingName+',' From tblHoldings h
Inner Join tblShopHolding sh on h.HoldingId = sh.HoldingId
Where sh.ShopId =s.ShopId and sh.FloorId=s.FloorId
Order By h.FloorId For XML PATH('')) as nvarchar(200)) 'HoldingNo'
From tblShops s
Inner Join tblFloors f on s.FloorId = f.FloorId 
Inner Join tblShopHolding sh on s.ShopId= sh.ShopId
Inner Join tblHoldings h on sh.HoldingId = h.HoldingId
where s.OrganizationId={0} {1}
) t1", User.OrgId, param);

                dropdowns=  db.Database.SqlQuery<Dropdown>(query).ToList();
            }
            catch (Exception ex)
            {
            }

            return Json(dropdowns);
        }
        #endregion

        #region Validator Values Methods
        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult GetFundBankBalance(int bankId, long fundInfoId)
        {
            var value = Checker.GetFundBankBalance(bankId, fundInfoId,User.OrgId);
            return Json(value);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetFundCashBalance(long fundInfoId)
        {
            var value = Checker.GetFundCashBalance(fundInfoId, User.OrgId);
            return Json(value);
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}