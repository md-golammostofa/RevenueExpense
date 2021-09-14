using RevenueAndExpense.BO.ViewModels;
using RevenueAndExpense.BO.Models;
using RevenueAndExpense.DAL.DataBaseContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using RevenueAndExpense.DAL.Security;
using RevenueAndExpense.BLL.Filters;
using RevenueAndExpense.BLL.Utility;
using RevenueAndExpense.BO.ReportModels;
using Microsoft.Reporting.WebForms;
using System.Text;
using NumberConvertToBengaliWord;

namespace RevenueAndExpense.Controllers
{
    [CustomAuthorize]
    public class OrganizationController : BaseController
    {
        private ApplicationDbContext db;
        public OrganizationController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Organization

        #region Floor
        [HttpGet]
        public ActionResult GetFloor()
        {
            var data = (from f in db.tblFloors
                        join o in db.tblOrganizations on f.OrganizationId equals o.OrganizationId
                        where o.OrganizationId == User.OrgId
                        select new VmFloor
                        {
                            FloorId = f.FloorId,
                            FloorNo = f.FloorNo,
                            Remarks = f.Remarks,
                            EntryUser = f.EntryUser,
                            EntryDate = f.EntryDate,
                            OrganizationId = o.OrganizationId,
                            OrganizationName = o.OrganizationName
                        }).ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult GetFloorById(int id)
        {
            var data = (from f in db.tblFloors
                        join o in db.tblOrganizations on f.OrganizationId equals o.OrganizationId
                        where f.FloorId == id && o.OrganizationId == User.OrgId
                        select new VmFloor
                        {
                            FloorId = f.FloorId,
                            FloorNo = f.FloorNo,
                            Remarks = f.Remarks,
                            EntryUser = f.EntryUser,
                            EntryDate = f.EntryDate,
                            OrganizationId = o.OrganizationId,
                            OrganizationName = o.OrganizationName
                        }).FirstOrDefault();

            return Json(data);
        }

        [HttpPost]
        public ActionResult SaveFloor(VmFloor model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.FloorId <= 0)
                    {
                        Floor floor = new Floor()
                        {
                            FloorNo = model.FloorNo,
                            OrganizationId = User.OrgId,
                            Remarks = model.Remarks,
                            EntryUser = User.UserName,
                            EntryDate = DateTime.Now
                        };
                        db.tblFloors.Add(floor);
                        IsSuccess = true;
                    }
                    else
                    {
                        var floorInDb = db.tblFloors.SingleOrDefault(f => f.FloorId == model.FloorId && f.OrganizationId == User.OrgId);
                        if (floorInDb != null)
                        {
                            floorInDb.FloorNo = model.FloorNo;
                            floorInDb.Remarks = model.Remarks;
                            floorInDb.UpdateBy = User.UserName;
                            floorInDb.UpdateDate = DateTime.Now;
                            IsSuccess = true;
                        }
                    }
                    db.SaveChanges();

                }
                catch (Exception)
                {
                    IsSuccess = false;
                }
            }
            return Json(IsSuccess);
        }

        [HttpPost]
        public ActionResult DeleteFloor(int FloorId)
        {
            bool IsSuccess = false;
            try
            {
                if (FloorId > 0)
                {
                    var floorIdDb = db.tblFloors.FirstOrDefault(f => f.FloorId == FloorId && f.OrganizationId == User.OrgId);
                    if (floorIdDb != null)
                    {
                        db.tblFloors.Remove(floorIdDb);
                        db.SaveChanges();
                        IsSuccess = true;
                    }
                }
            }
            catch (Exception)
            {
                IsSuccess = false;
            }
            return Json(IsSuccess);
        }
        #endregion

        #region Holding
        public ActionResult GetHoldings()
        {
            var holdings = new List<VmHolding>();
            try
            {
                ViewBag.FloorList = db.tblFloors.Where(f => f.OrganizationId == User.OrgId).Select(f =>
                new SelectListItem { Text = "ফ্লোরঃ" + f.FloorNo, Value = f.FloorId.ToString() }
                ).ToList();

                string query = string.Format(@"Select h.*,f.FloorNo,o.OrganizationName From tblHoldings h
Inner Join tblFloors f on h.FloorId = f.FloorId
Inner Join tblOrganizations o on h.OrganizationId = o.OrganizationId
where 1 = 1 and  o.OrganizationId={0}", User.OrgId);

                holdings = db.Database.SqlQuery<VmHolding>(query).ToList();
            }
            catch (Exception ex)
            {

            }
            return View(holdings);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetHoldingsBySearch(long floorId = 0, string holdingNo = "")
        {
            List<VmHolding> holdingList = new List<VmHolding>();
            try
            {
                string floor = floorId > 0 ? string.Format(@" and f.FloorId ={0}", floorId) : "";
                string holding = holdingNo.Trim() == "" ? "" : string.Format(@" and h.HoldingName =N'{0}'", holdingNo);

                string query = string.Format(@"Select h.*,f.FloorNo,o.OrganizationName From tblHoldings h
Inner Join tblFloors f on h.FloorId = f.FloorId
Inner Join tblOrganizations o on h.OrganizationId = o.OrganizationId
where 1 = 1 and  o.OrganizationId={0} {1} {2}", User.OrgId, floor, holding);
                holdingList = db.Database.SqlQuery<VmHolding>(query).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }

            return PartialView(holdingList);
        }
        [HttpPost]
        public ActionResult GetHoldingById(int Id)
        {
            VmHolding data = db.tblHoldings.Include("tblFloors").Include("tblOrganizations").Where(h => h.HoldingId == Id && h.OrganizationId == User.OrgId).Select(hh => new VmHolding { HoldingId = hh.HoldingId, HoldingName = hh.HoldingName, Remarks = hh.Remarks, EntryUser = hh.EntryUser, EntryDate = hh.EntryDate, UpdateBy = hh.UpdateBy, UpdateDate = hh.UpdateDate, FloorId = hh.FloorId, FloorNo = hh.Floor.FloorNo, OrganizationId = hh.OrganizationId, OrganizationName = hh.Organization.OrganizationName }).FirstOrDefault();

            return Json(data);
        }

        [HttpPost]
        public ActionResult SaveHolding(VmHolding model)
        {
            bool isSuccess = false;
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.HoldingId <= 0)
                    {
                        Holding holding = new Holding
                        {
                            HoldingName = model.HoldingName,
                            Remarks = model.Remarks,
                            FloorId = model.FloorId.Value,
                            OrganizationId = User.OrgId,
                            EntryUser = User.UserName,
                            EntryDate = DateTime.Now
                        };
                        db.tblHoldings.Add(holding);
                        isSuccess = true;
                    }
                    else
                    {
                        var holdingInDb = db.tblHoldings.FirstOrDefault(h => h.HoldingId == model.HoldingId && h.OrganizationId == User.OrgId);
                        if (holdingInDb != null)
                        {
                            holdingInDb.HoldingName = model.HoldingName;
                            holdingInDb.Remarks = model.Remarks;
                            holdingInDb.FloorId = model.FloorId.Value;
                            holdingInDb.UpdateBy = User.UserName;
                            holdingInDb.UpdateDate = DateTime.Now;
                            isSuccess = true;
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            return Json(isSuccess);
        }

        [HttpPost]
        public ActionResult DeleteHolding(int Id)
        {
            bool IsSuccess = false;
            try
            {
                if (Id > 0)
                {
                    var holdingInDb = db.tblHoldings.FirstOrDefault(h => h.HoldingId == Id && h.OrganizationId == User.OrgId);
                    if (holdingInDb != null)
                    {
                        db.tblHoldings.Remove(holdingInDb);
                        db.SaveChanges();
                        IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
            }
            return Json(IsSuccess);
        }

        #endregion

        #region Shop
        [HttpGet]
        public ActionResult GetShops()
        {
            List<VmShop> shops = new List<VmShop>();
            try
            {
                ViewBag.FloorList = db.tblFloors.Where(f => f.OrganizationId == User.OrgId).Select(f =>
                new SelectListItem { Text = "ফ্লোরঃ" + f.FloorNo, Value = f.FloorId.ToString() }
                ).ToList();

                string query = string.Format(@"Select ShopId,ShopName,ProprietorName,LeaseholderPhone,FloorId,FloorNo 'FloorName',StateStatus,SUBSTRING(HoldingNo,0,Len(HoldingNo)) 'HoldingNo',EntryUser 
From (Select s.ShopId,s.ShopName,s.ProprietorName,s.LeaseholderPhone,s.FloorId,f.FloorNo,s.StateStatus,s.EntryUser,
Cast((Select h.HoldingName+',' From tblHoldings h
Inner Join tblShopHolding sh on h.HoldingId = sh.HoldingId
Where sh.ShopId =s.ShopId and sh.FloorId=s.FloorId
Order By h.FloorId For XML PATH('')) as nvarchar(200)) 'HoldingNo'
From tblShops s
left Join tblFloors f on s.FloorId = f.FloorId where s.OrganizationId={0}) t1
", User.OrgId);
                shops = db.Database.SqlQuery<VmShop>(query).ToList();
            }
            catch (Exception ex)
            {

            }
            return View(shops);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetShopsBySearch(int floorId, string holdingNo, long shopId)
        {
            List<VmShop> shopList = new List<VmShop>();
            try
            {
                var floor = floorId > 0 ? string.Format(@" and f.FloorId={0}", floorId) : "";
                var holding = !string.IsNullOrEmpty(holdingNo) ? string.Format(@" and HoldingNo Like N'%{0}%' ", holdingNo) : "";
                var shop = shopId > 0 ? string.Format(@" and s.ShopId={0}", shopId) : "";
                var param = floor + shop;
                var param2 = holding;

                string query = string.Format(@"Select Distinct ShopId,ShopName,ProprietorName,MobileNo,FloorId,FloorNo 'FloorName',StateStatus,SUBSTRING(HoldingNo,0,Len(HoldingNo)) 'HoldingNo',EntryUser 
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
) t1 Where 1=1 {2}", User.OrgId, param, param2);

                shopList = db.Database.SqlQuery<VmShop>(query).ToList();
            }
            catch (Exception ex)
            {
            }
            return PartialView(shopList);
        }

        [HttpPost]
        public ActionResult SaveShop(VmShop Shop, List<VmShopHolding> Holdings, List<VmShopCharge> Charges)
        {
            bool isSuccess = false;
            try
            {
                if (ModelState.IsValid && Holdings.Count > 0 && Charges.Count > 0)
                {
                    if (Shop.ShopId <= 0)
                    {
                        var thisDate = DateTime.Now;
                        Shop shop = new Shop()
                        {
                            ShopName = Shop.ShopName,
                            ProprietorName = Shop.ProprietorName,
                            MobileNo = Shop.MobileNo,
                            Email = Shop.Email,
                            HomeAddress = Shop.HomeAddress,
                            RegistrationNo = Shop.RegistrationNo,
                            StateStatus = Shop.StateStatus,
                            FloorId = Shop.FloorId,
                            EntryDate = thisDate,
                            EntryUser = User.UserName,
                            OrganizationId = User.OrgId,
                            LandOwner = Shop.LandOwner,
                            LandOwnerAddress = Shop.LandOwnerAddress,
                            LandOwnerMobile = Shop.LandOwnerMobile,
                            LeaseholderName = Shop.LeaseholderName,
                            LeaseholderAddress = Shop.LeaseholderAddress,
                            LeaseholderPhone = Shop.LeaseholderPhone,
                            NIDNo = Shop.NIDNo,
                            MeterNo = Shop.MeterNo,
                            Remarks = Shop.Remarks,
                            ShopSize = Shop.ShopSize
                        };

                        db.tblShops.Add(shop);
                        isSuccess = db.SaveChanges() > 0;

                        var newShopId = db.tblShops.Where(s => s.OrganizationId == User.OrgId && s.FloorId == Shop.FloorId && DbFunctions.TruncateTime(s.EntryDate) == DbFunctions.TruncateTime(thisDate)).OrderByDescending(s => s.ShopId).FirstOrDefault();

                        if (newShopId != null && isSuccess)
                        {
                            // Add Holding
                            foreach (var holding in Holdings)
                            {
                                var sh = new ShopHolding();
                                sh.FloorId = Shop.FloorId;
                                sh.OrganizationId = User.OrgId;
                                sh.ShopId = newShopId.ShopId;
                                sh.HoldingId = holding.HoldingId;
                                sh.EntryUser = User.UserName;
                                sh.EntryDate = DateTime.Now;
                                db.tblShopHolding.Add(sh);
                            }
                            // Add Shop Charges...

                            foreach (var charge in Charges)
                            {
                                var sc = new ShopCharge();
                                sc.ShopId = newShopId.ShopId;
                                sc.FundInfoId = charge.FundInfoId;
                                sc.FundDetailId = charge.FundDetailId;
                                sc.ChargeAmountEN = charge.ChargeAmountEN;
                                sc.ChargeAmountBN = charge.ChargeAmountBN;
                                sc.ChargeName = charge.ChargeName;
                                sc.OrganizationId = User.OrgId;
                                sc.EntryUser = User.UserName;
                                sc.EntryDate = DateTime.Now;
                                db.tblShopCharges.Add(sc);
                            }
                            isSuccess = db.SaveChanges() > 0;
                        }
                    }

                    // Edit Part
                    else
                    {
                        var shopInDb = db.tblShops.FirstOrDefault(s => s.ShopId == Shop.ShopId && s.OrganizationId == User.OrgId);
                        if (shopInDb != null)
                        {
                            shopInDb.ShopName = Shop.ShopName;
                            var floorIdInDb = shopInDb.FloorId;
                            shopInDb.ProprietorName = Shop.ProprietorName;
                            shopInDb.MobileNo = Shop.MobileNo;
                            shopInDb.Email = Shop.Email;
                            shopInDb.HomeAddress = Shop.HomeAddress;
                            shopInDb.RegistrationNo = Shop.RegistrationNo;
                            shopInDb.StateStatus = Shop.StateStatus;

                            shopInDb.LandOwner = Shop.LandOwner;
                            shopInDb.LandOwnerAddress = Shop.LandOwnerAddress;
                            shopInDb.LandOwnerMobile = Shop.LandOwnerMobile;
                            shopInDb.LeaseholderName = Shop.LeaseholderName;
                            shopInDb.LeaseholderAddress = Shop.LeaseholderAddress;
                            shopInDb.LeaseholderPhone = Shop.LeaseholderPhone;
                            shopInDb.NIDNo = Shop.NIDNo;
                            shopInDb.MeterNo = Shop.MeterNo;
                            shopInDb.Remarks = Shop.Remarks;
                            shopInDb.ShopSize = Shop.ShopSize;

                            if (floorIdInDb != Shop.FloorId)
                            {
                                var shData = db.tblShopHolding.Where(sh => sh.ShopId == Shop.ShopId && sh.FloorId == Shop.FloorId && sh.OrganizationId == User.OrgId).ToList();
                                if (shData != null)
                                    db.tblShopHolding.RemoveRange(shData);
                            }

                            string hid = string.Empty;
                            foreach (var item in Holdings)
                            {
                                hid += item.HoldingId.ToString() + ",";
                                //db.tblShopHolding.Where(sh => sh.)
                            }

                            string fd = string.Empty;
                            foreach (var item in Charges)
                            {
                                fd += item.FundDetailId.ToString() + ",";
                            }

                            hid = hid.Substring(0, hid.Length - 1);
                            fd = fd.Substring(0, fd.Length - 1);

                            string delQ = string.Format(@"SET NOCOUNT off; Delete from tblShopHolding where ShopId = {0} and FloorId ={1} and HoldingId not in ({2}) and OrganizationId={3} ; Delete From tblShopCharges Where ShopId = {0} and FundDetailId not in ({4}) and OrganizationId={3} select @@ROWCOUNT", Shop.ShopId, Shop.FloorId, hid, User.OrgId, fd);
                            var delval = db.Database.SqlQuery<int>(delQ).Single();


                            // write code here..
                            foreach (var item in Holdings)
                            {
                                var holdingInDb = db.tblShopHolding.FirstOrDefault(sh => sh.HoldingId == item.HoldingId && sh.OrganizationId == User.OrgId);
                                if (holdingInDb == null)
                                {
                                    ShopHolding shopHolding = new ShopHolding
                                    {
                                        HoldingId = item.HoldingId,
                                        FloorId = Shop.FloorId,
                                        ShopId = Shop.ShopId,
                                        OrganizationId = shopInDb.OrganizationId,
                                        EntryUser = User.UserName,
                                        EntryDate = DateTime.Now
                                    };
                                    db.tblShopHolding.Add(shopHolding);
                                }
                            }
                            foreach (var item in Charges)
                            {
                                var chargeInDb = db.tblShopCharges.FirstOrDefault(sc => sc.ShopId == Shop.ShopId && sc.FundDetailId == item.FundDetailId);
                                if (chargeInDb != null)
                                {
                                    chargeInDb.ChargeAmountEN = item.ChargeAmountEN;
                                    chargeInDb.ChargeAmountBN = item.ChargeAmountBN;
                                    chargeInDb.UpdateBy = User.UserName;
                                    chargeInDb.UpdateDate = DateTime.Now;
                                }
                                else
                                {
                                    ShopCharge shopCharge = new ShopCharge
                                    {
                                        ShopId = Shop.ShopId,
                                        FundInfoId = item.FundInfoId,
                                        FundDetailId = item.FundDetailId,
                                        OrganizationId = User.OrgId,
                                        ChargeName = item.ChargeName,
                                        ChargeAmountEN = item.ChargeAmountEN,
                                        ChargeAmountBN = item.ChargeAmountBN,
                                        EntryUser = User.UserName,
                                        EntryDate = DateTime.Now
                                    };
                                    db.tblShopCharges.Add(shopCharge);
                                }
                            }

                            isSuccess = db.SaveChanges() > 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return Json(isSuccess);
        }

        [HttpPost]
        public ActionResult GetShopDataForModification(int shopid, int floorid)
        {
            bool isSuccess = false;
            VmShop shop = new VmShop();
            List<VmShopHolding> shopHoldings = new List<VmShopHolding>();
            List<VmShopCharge> shopCharges = new List<VmShopCharge>();
            try
            {
                if (shopid > 0 && floorid > 0)
                {
                    shop = db.Database.SqlQuery<VmShop>(string.Format(@"Select s.ShopId,s.ShopName,s.ProprietorName,s.MobileNo,
s.Email,s.HomeAddress,s.RegistrationNo,s.StateStatus,f.FloorId,f.FloorNo 'FloorName',
s.LandOwner,S.LandOwnerAddress,S.LandOwnerMobile,S.LeaseholderName,S.LeaseholderAddress,S.LandOwnerMobile,
S.NIDNo,S.MeterNo,S.Remarks,S.ShopSize
From tblShops s 
Inner Join tblFloors f on s.FloorId = f.FloorId
Where s.OrganizationId = {1} and s.ShopId={0}", shopid, User.OrgId)).Single();

                    shopHoldings = db.Database.SqlQuery<VmShopHolding>(string.Format(@"Select sh.ShopId,h.HoldingId,h.HoldingName 'HoldingNo' From tblHoldings h
inner join tblShopHolding sh on h.HoldingId = sh.HoldingId
where sh.OrganizationId = {0} and sh.ShopId = {1} and sh.FloorId={2}", User.OrgId, shopid, floorid)).ToList();

                    shopCharges = db.Database.SqlQuery<VmShopCharge>(string.Format(@"Select sc.FundInfoId,f.FundNameBN,fd.FundDetailId,fd.FundDetailNameBN,sc.ChargeAmountEN From tblShopCharges sc
Inner Join tblShops s on sc.ShopId = s.ShopId
Inner Join tblFundInfo f on sc.FundInfoId = f.FundInfoId
Inner Join tblFundDetail fd on sc.FundDetailId = fd.FundDetailId
Where 1= 1 and sc.OrgnizationId = {0} and s.ShopId = {1}", User.OrgId, shop.ShopId)).ToList();

                    if (shop == null || shopHoldings.Count == 0)
                    {
                        isSuccess = false;
                    }
                    else
                    {
                        isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return Json(new { exec = isSuccess, shop = shop, shopHoldings = shopHoldings, shopCharges = shopCharges });
        }

        [HttpPost]
        public ActionResult DeleteShop(int shopId)
        {
            bool isSuccess = false;
            try
            {
                if (shopId > 0)
                {
                    var shopHoldings = db.tblShopHolding.Where(sh => sh.ShopId == shopId && sh.OrganizationId == User.OrgId).ToList();
                    if (shopHoldings.Count > 0)
                        db.tblShopHolding.RemoveRange(shopHoldings);
                    var shop = db.tblShops.FirstOrDefault(s => s.ShopId == shopId && s.OrganizationId == User.OrgId);
                    if (shop != null)
                    {
                        db.tblShops.Remove(shop);
                    }
                    db.SaveChanges();
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            return Json(isSuccess);
        }

        [HttpGet]
        public ActionResult CreateShop(long? shopid, long? floorid)
        {
            VmShopModel model = new VmShopModel();
            VmShop shop = new VmShop();
            List<VmShopHolding> shopHoldings = new List<VmShopHolding>();
            List<VmShopCharge> ShopCharges = new List<VmShopCharge>();
            ViewBag.DDLStateStatus = new List<SelectListItem>(){
                new SelectListItem{Text="অবস্থা সিলেক্ট করুন",Value="" },
                new SelectListItem{Text="চালু আছে",Value="Active"  },
                new SelectListItem{Text="বন্ধ আছে",Value="Inactive"  }
            };
            try
            {
                if (shopid > 0 && floorid > 0)
                {
                    shop = db.Database.SqlQuery<VmShop>(string.Format(@"Select s.ShopId,s.ShopName,s.ProprietorName,s.MobileNo,
s.Email,s.HomeAddress,s.RegistrationNo,s.StateStatus,f.FloorId,f.FloorNo 'FloorName',
s.LandOwner,s.LandOwnerMobile,s.LandOwnerAddress,s.LeaseholderName,s.LeaseholderAddress,s.LeaseholderPhone,
s.NIDNo,s.Remarks,s.ShopSize,s.MeterNo
From tblShops s 
Inner Join tblFloors f on s.FloorId = f.FloorId
Where s.OrganizationId = {1} and s.ShopId={0}", shopid, User.OrgId)).Single();

                    shopHoldings = db.Database.SqlQuery<VmShopHolding>(string.Format(@"Select sh.ShopId,h.HoldingId,h.HoldingName 'HoldingNo' From tblHoldings h
inner join tblShopHolding sh on h.HoldingId = sh.HoldingId
where sh.OrganizationId = {0} and sh.ShopId = {1} and sh.FloorId={2}", User.OrgId, shopid, floorid)).ToList();

                    ShopCharges = db.Database.SqlQuery<VmShopCharge>(string.Format(@"Select sc.*,s.ShopName,fd.FundDetailName,fd.FundDetailNameBN,fi.FundName,fi.FundNameBN From tblShopCharges sc
Inner Join tblFundDetail fd on sc.FundDetailId = fd.FundDetailId
Inner Join tblFundInfo fi on sc.FundInfoId = fi.FundInfoId
Inner Join tblShops s on sc.ShopId = s.ShopId
Where s.OrganizationId={0} and s.ShopId={1} and s.FloorId={2}", User.OrgId, shopid, floorid)).ToList();

                    ViewBag.Title = "Update Shop";
                    ViewBag.PageTitle = "শপ আপডেট করুন";
                    model.Shop = shop;
                    model.ShopHoldings = shopHoldings;
                    model.ShopCharges = ShopCharges;
                    return View(model);
                }
                else
                {
                    ViewBag.Title = "Create Shop";
                    ViewBag.PageTitle = "নতুন শপ তৈরি করুন";
                    model.Shop = shop;
                    model.ShopHoldings = shopHoldings;
                    model.ShopCharges = ShopCharges;
                    return View(model);
                }
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        [HttpPost]
        public ActionResult GetChargesByShopId(long? shopid)
        {
            var data = (from sc in db.tblShopCharges
                        join s in db.tblShops on sc.ShopId equals s.ShopId
                        join fi in db.tblFundInfo on sc.FundInfoId equals fi.FundInfoId
                        join fd in db.tblFundDetail on sc.FundDetailId equals fd.FundDetailId
                        where sc.OrganizationId == User.OrgId && sc.ShopId == shopid
                        select new VmShopCharge
                        {
                            FundInfoId = fi.FundInfoId,
                            FundNameBN = fi.FundNameBN,
                            FundDetailId = fd.FundDetailId,
                            FundDetailNameBN = fd.FundDetailNameBN,
                            ChargeAmountEN = sc.ChargeAmountEN,
                            ShopName = s.ShopName,
                            ProprietorName = s.ProprietorName

                        }).ToList();

            return PartialView("_ChargesByShopId", data);

        }
        #endregion

        #region FundInfo
        [HttpGet]
        public ActionResult GetFundList()
        {
            List<VmFund> data = new List<VmFund>();
            try
            {
                data = db.Database.SqlQuery<VmFund>(string.Format(@"Select f.*,o.OrganizationName From tblFundInfo f
Inner Join tblOrganizations o On f.OrganizationId = o.OrganizationId
Where 1= 1 and o.OrganizationId = {0}", User.OrgId)).ToList();
            }
            catch (Exception ex)
            {

            }
            return View(data);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveFundInfo(VmFund model)
        {
            var isSuccess = false;
            try
            {

                if (ModelState.IsValid)
                {
                    if (model.FundInfoId <= 0)
                    {
                        FundInfo fundInfo = new FundInfo()
                        {
                            FundName = model.FundName,
                            FundNameBN = model.FundNameBN,
                            Remarks = model.Remarks,
                            EntryUser = User.UserName,
                            EntryDate = DateTime.Now,
                            OrganizationId = User.OrgId,
                            IsActive = true
                        };
                        db.tblFundInfo.Add(fundInfo);
                        isSuccess = true;
                    }
                    else
                    {
                        var fundInDb = db.tblFundInfo.Where(f => f.FundInfoId == model.FundInfoId && f.OrganizationId == User.OrgId).FirstOrDefault();

                        if (fundInDb != null)
                        {
                            fundInDb.Remarks = model.Remarks;
                            fundInDb.FundNameBN = model.FundNameBN;
                            fundInDb.FundName = model.FundName;
                            isSuccess = true;
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                //
                isSuccess = false;
            }
            return Json(isSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetFundInfoById(int Id)
        {
            VmFund data = new VmFund();
            try
            {
                if (Id > 0)
                {
                    data = db.tblFundInfo.Where(f => f.FundInfoId == Id && f.OrganizationId == User.OrgId).Select(
                        f => new VmFund { FundName = f.FundName, FundNameBN = f.FundNameBN, FundInfoId = f.FundInfoId, Remarks = f.Remarks }
                        ).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
            return Json(data);
        }
        #endregion

        #region Fund Details
        [HttpGet]
        public ActionResult GetFundDetailList(long? fundId, string fundDetailName, string type, string flag)
        {
            List<VmFundDetail> data = new List<VmFundDetail>();
            try
            {
                if (string.IsNullOrEmpty(flag))
                {
                    data = db.tblFundDetail.Include("tblFundInfo").Where(fd => fd.OrganizationId == User.OrgId)
                        .Select(fd => new VmFundDetail
                        {
                            FundDetailId = fd.FundDetailId,
                            FundDetailName = fd.FundDetailName,
                            FundDetailNameBN = fd.FundDetailNameBN,
                            FundInfoId = fd.FundInfo.FundInfoId,
                            FundName = fd.FundInfo.FundNameBN,
                            Type = fd.Type,
                            TypeBN = fd.TypeBN,
                            OrganizationName = fd.FundInfo.Organization.OrganizationName,
                            Remarks = fd.Remarks,
                            EntryUser = fd.EntryUser,
                            IsMonthlyChargeable = fd.IsMonthlyChargeable
                        }).ToList();

                    return View(data);
                }
                else
                {
                    data = db.tblFundDetail.Include("tblFundInfo").Where(fd => fd.OrganizationId == User.OrgId && (fundId == null || fundId == 0 || fd.FundInfo.FundInfoId == fundId) && (string.IsNullOrEmpty(fundDetailName) || fd.FundDetailNameBN.ToLower().Contains(fundDetailName.Trim().ToLower())) && (string.IsNullOrEmpty(type) || fd.Type == type))
                        .Select(fd => new VmFundDetail
                        {
                            FundDetailId = fd.FundDetailId,
                            FundDetailName = fd.FundDetailName,
                            FundDetailNameBN = fd.FundDetailNameBN,
                            FundInfoId = fd.FundInfo.FundInfoId,
                            FundName = fd.FundInfo.FundNameBN,
                            Type = fd.Type,
                            TypeBN = fd.TypeBN,
                            OrganizationName = fd.FundInfo.Organization.OrganizationName,
                            Remarks = fd.Remarks,
                            EntryUser = fd.EntryUser,
                            IsMonthlyChargeable = fd.IsMonthlyChargeable
                        }).ToList();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveFundDetail(VmFundDetail model)
        {
            bool isSuccess = false;
            try
            {
                if (ModelState.IsValid)
                {
                    var thisTime = DateTime.Now;
                    string billNo = thisTime.Year.ToString().Substring(2) +
                                thisTime.Month.ToString().PadLeft(2, '0') + thisTime.ToString("dd") + thisTime.ToString("HH") + thisTime.ToString("mm") + thisTime.ToString("ss") + thisTime.ToString("ff");

                    if (model.FundDetailId <= 0)
                    {

                        FundDetail fundDetail = new FundDetail
                        {
                            FundDetailName = model.FundDetailName,
                            FundDetailNameBN = model.FundDetailNameBN,
                            FundInfoId = model.FundInfoId.Value,
                            Type = model.Type,
                            TypeBN = model.TypeBN,
                            Remarks = model.Remarks,
                            EntryUser = User.UserName,
                            EntryDate = thisTime,
                            IsActive = true,
                            OrganizationId = User.OrgId,
                            AmountBN = model.AmountBN,
                            AmountEN = model.AmountEN,
                            OpeningBalance = model.OpeningBalance,
                            IsFan = model.IsFan,
                            IsMonthlyChargeable = model.IsMonthlyChargeable
                        };
                        db.tblFundDetail.Add(fundDetail);
                        isSuccess = db.SaveChanges() > 0;

                        if (model.OpeningBalance != null && model.OpeningBalance > 0)
                        {

                            var fdId = db.tblFundDetail.Where(f => f.FundInfoId == model.FundInfoId && f.AmountEN == f.AmountEN && f.OrganizationId == User.OrgId && DbFunctions.TruncateTime(f.EntryDate) == DbFunctions.TruncateTime(thisTime)).OrderByDescending(f => f.FundDetailId).FirstOrDefault();

                            RevenueExpense revenueExpense = new RevenueExpense
                            {
                                FundDetailId = fdId.FundDetailId,
                                FundInfoId = model.FundInfoId.Value,
                                IsOpeningBalance = true,
                                Amount = model.OpeningBalance.Value,
                                AmountBN = Utility.ConvertEnNumToBnNum(model.OpeningBalance.Value.ToString()),
                                PayFromOrPayTo = "N/A",
                                StateStatus = "Paid",
                                EntryUser = User.UserName,
                                EntryDate = thisTime,
                                OrganizationId = User.OrgId,
                                Remark = "Opening Balance",
                                RevExName = fdId.FundDetailNameBN,
                                BillDate = thisTime.Date,
                                BillNo = billNo,
                            };
                            db.tblRevenueExpenses.Add(revenueExpense);
                            isSuccess = db.SaveChanges() > 0;
                        }
                    }
                    else
                    {
                        var funDetailInDb = db.tblFundDetail.FirstOrDefault(f => f.FundDetailId == model.FundDetailId && f.OrganizationId == User.OrgId);
                        if (funDetailInDb != null)
                        {
                            funDetailInDb.Type = model.Type;
                            funDetailInDb.TypeBN = model.TypeBN;
                            funDetailInDb.FundDetailName = model.FundDetailName;
                            funDetailInDb.FundDetailNameBN = model.FundDetailNameBN;
                            funDetailInDb.UpdateBy = User.UserName;
                            funDetailInDb.UpdateDate = DateTime.Now;
                            funDetailInDb.Remarks = model.Remarks;
                            funDetailInDb.AmountBN = model.AmountBN;
                            funDetailInDb.AmountEN = model.AmountEN;
                            funDetailInDb.IsFan = model.IsFan;
                            funDetailInDb.OpeningBalance = model.OpeningBalance;
                            funDetailInDb.IsMonthlyChargeable = model.IsMonthlyChargeable;

                            if (model.OpeningBalance != null && model.OpeningBalance.Value > 0)
                            {
                                // Update OB
                                var reEx = db.tblRevenueExpenses.FirstOrDefault(r => r.FundDetailId == model.FundDetailId && r.IsOpeningBalance == true && r.FundInfoId == model.FundInfoId && r.OrganizationId == User.OrgId);
                                if (reEx != null)
                                {
                                    reEx.Amount = model.OpeningBalance.Value;
                                    reEx.AmountBN = Utility.ConvertEnNumToBnNum(model.OpeningBalance.Value.ToString());
                                }
                                else
                                {
                                    RevenueExpense revenueExpense = new RevenueExpense
                                    {
                                        FundDetailId = funDetailInDb.FundDetailId,
                                        FundInfoId = model.FundInfoId.Value,
                                        IsOpeningBalance = true,
                                        Amount = model.OpeningBalance.Value,
                                        AmountBN = Utility.ConvertEnNumToBnNum(model.OpeningBalance.Value.ToString()),
                                        PayFromOrPayTo = "N/A",
                                        StateStatus = "Paid",
                                        EntryUser = User.UserName,
                                        EntryDate = thisTime,
                                        OrganizationId = User.OrgId,
                                        Remark = "Opening Balance",
                                        RevExName = funDetailInDb.FundDetailNameBN,
                                        BillDate = thisTime.Date,
                                        BillNo = billNo
                                    };
                                    db.tblRevenueExpenses.Add(revenueExpense);
                                }
                            }
                            else
                            {
                                // Remove OB
                                //var reEx = db.tblRevenueExpenses.FirstOrDefault(r => r.FundDetailId == model.FundDetailId && r.IsOpeningBalance == true && r.FundInfoId == model.FundInfoId && r.OrganizationId == User.OrgId);
                                //if (reEx != null)
                                //{
                                //    db.tblRevenueExpenses.Remove(reEx);
                                //}
                            }

                            isSuccess = db.SaveChanges() > 0;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            return Json(isSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetFundDetailById(long Id)
        {
            var data = db.tblFundDetail.Where(fd => fd.FundDetailId == Id && fd.OrganizationId == User.OrgId).Select(fd => new VmFundDetail
            {
                FundDetailId = fd.FundDetailId,
                FundInfoId = fd.FundInfoId,
                FundDetailName = fd.FundDetailName,
                FundDetailNameBN = fd.FundDetailNameBN,
                Remarks = fd.Remarks,
                Type = fd.Type,
                TypeBN = fd.TypeBN,
                AmountBN = fd.AmountBN,
                AmountEN = fd.AmountEN,
                IsFan = fd.IsFan,
                OpeningBalance = fd.OpeningBalance,
                IsMonthlyChargeable = fd.IsMonthlyChargeable
            }).FirstOrDefault();

            return Json(data);
        }
        #endregion

        #region Shops monthly charges

        [HttpGet]
        public ActionResult ShopPanel()
        {
            ViewBag.ddlShopList = db.tblShops.Where(s => s.OrganizationId == User.OrgId).Select(s => new SelectListItem
            {
                Text = s.ShopName,
                Value = s.ShopId.ToString()
            }).ToList();

            ViewBag.ddlMonthAndYear = Utility.GetMonthAndYearListBN(11);

            ViewBag.ddlStateStatus = new List<SelectListItem> {
                new SelectListItem{Text="পরিশোধ হয়েছে",Value="Paid" },
                new SelectListItem{Text="বকেয়া রয়েছে", Value="Pending"}
            };

            ViewBag.ddlFloors = db.tblFloors.Where(f => f.OrganizationId == User.OrgId).Select(o => new SelectListItem
            {
                Value = o.FloorId.ToString(),
                Text = o.FloorNo
            }).ToList();

            return View();
        }

        [HttpGet]
        public ActionResult GetShopMonthlyCharges(long? shopId, long? floorId, string date, string status, string holding)
        {
            if (status == "initialize")
            {
                date = DateTime.Now.Date.ToString("dd-MMM-yyyy");
                status = "Pending";
            }
            List<VmShopMonthlyBill> billList = new List<VmShopMonthlyBill>();
            string query = string.Format(@"Select Distinct S.ShopId,msc.[Month],msc.[Year],F.FloorId,F.FloorNo,S.ShopName,S.MobileNo,S.HomeAddress,S.Email,dbo.fnGetBnNumerical(Amount) 'AmountBN',msc.Amount 'AmountEN',(dbo.fnGetMonthNameBN(msc.[Month])+','+dbo.fnGetBnNumerical(msc.[Year])) 'MonthAndYearBN',(Case When msc.StateStatus='Pending' then N'বকেয়া রয়েছে' When msc.StateStatus='Paid' then N'পরিশোধ হয়েছে' Else '' End) 'StateStatusBN',msc.StateStatus,([dbo].[fnGetShopHoldings](ISNULL(s.ShopId,0),ISNULL(f.FloorId,0),ISNULL({0},0))) 'Holding' From tblShops s
            Inner Join tblFloors f on S.FloorId =f.FloorId
            Inner Join tblShopHolding sh on s.ShopId = sh.ShopId
            Inner Join tblHoldings h on sh.HoldingId = h.HoldingId
            Inner Join (select Cast(SUM(Amount) as decimal)  'Amount',[Month],[Year],ShopId,StateStatus from tblMonthWiseShopCharges 
            Where OrganizationId ={0}
            Group By [Month], [Year], ShopId,StateStatus )msc
            on s.ShopId = msc.ShopId 
            Where s.OrganizationId = {0} {1}", User.OrgId, ShopParam(shopId, floorId, date, status, holding));
            billList = db.Database.SqlQuery<VmShopMonthlyBill>(query).ToList();
            return View(billList);
        }

        // Shop Due Report
        public ActionResult GetFloorAndShopWiseDues(int month=0, int year=0,long floorId=0, string format="PDF")
        {
            //format = "PDF";
            var details = db.Database.SqlQuery<FloorAndShopWiseDueDetail>(string.Format(@"Exec spFloorAndShopWiseDueDetail {0},{1},{2},{3}",month,year,floorId,User.OrgId)).ToList();
            List<FloorAndShopWiseDueSummery> summeryList = new List<FloorAndShopWiseDueSummery>();
            if(details.Count > 0)
            {
                FloorAndShopWiseDueSummery summery = new FloorAndShopWiseDueSummery()
                {
                    TotalCurrentBill = details.Sum(s => s.Amount),
                    TotalOtherCharges = details.Sum(s => s.OtherCharges),
                    TotalAmount = details.Sum(s=> s.Amount),
                    TotalFine = details.Sum(s=> s.Fine),
                    TotalReconFee = details.Sum(s=> s.ReconFee),
                    TotalGrossAmount = details.Sum(s=> s.GrossAmount)
                    
                };
                summery.TotalCurrentBillBN = Utility.ConvertEnNumToBnNum(summery.TotalCurrentBill.ToString());
                summery.TotalOtherChargesBN = Utility.ConvertEnNumToBnNum(summery.TotalOtherCharges.ToString());
                summery.TotalAmountBN = Utility.ConvertEnNumToBnNum(summery.TotalAmount.ToString());
                summery.TotalFineBN = Utility.ConvertEnNumToBnNum(summery.TotalFine.ToString());
                summery.TotalReconFeeBN = Utility.ConvertEnNumToBnNum(summery.TotalReconFee.ToString());
                summery.TotalGrossAmountBN = Utility.ConvertEnNumToBnNum(summery.TotalGrossAmount.ToString());

                summery.TotalDueAmount = (summery.TotalCurrentBill + summery.TotalOtherCharges + summery.TotalAmount + summery.TotalFine + summery.TotalReconFee + summery.TotalGrossAmount);
                summery.TotalDueAmountBN = Utility.ConvertEnNumToBnNum(summery.TotalDueAmount.ToString());
                NumberToBengaliWord bengaliWord = new NumberToBengaliWord();
                summery.InWordBN= bengaliWord.NumToWord(Math.Round(summery.TotalGrossAmount).ToString());
                summeryList.Add(summery);
            }

            string path = string.Format(@"~/Reports/rptFloorAndShopWiseDues.rdlc");
            byte[] fileBytes = null;
            string fileMimeType = null;
            string rptType = format;
            GetReportFileByTwoDataSource(path, details, "FloorAndShopWiseDueDetail",summeryList, "FloorAndShopWiseDueSummery", rptType, out fileBytes, out fileMimeType);

            var fileName = "ফ্লোর এবং শপ অনুযায়ী বকেয়া রিপোর্ট_" + DateTime.Now.ToString("dd-MMM-yyyy") + (format == "PDF" ? ".pdf" : (format == "EXCEL" ? ".xls" : ".doc"));
            if (format == "PDF")
            {
                return File(fileBytes, fileMimeType);
            }
            else
            {
                return File(fileBytes, fileMimeType, fileName);
            }
        }

        [HttpPost]
        public ActionResult SaveShopMonthlyCharges(VmShopMonthlyChargeInfo model)
        {
            if (ModelState.IsValid && model.ChargeDetails != null && model.ChargeDetails.Count > 0)
            {
                DateTime month = Convert.ToDateTime(model.Date).AddMonths(1);
                int LastDayOfMonth = DateTime.DaysInMonth(month.Year, month.Month);
                List<MonthWiseShopCharge> listMonthWiseShopCharge = new List<MonthWiseShopCharge>();
                foreach (var item in model.ChargeDetails)
                {
                    string amountBN = Utility.ConvertEnNumToBnNum(item.Amount.ToString());

                    MonthWiseShopCharge monthWiseShopCharge = new MonthWiseShopCharge
                    {
                        FundInfoId = item.FundInfoId.Value,
                        ChargeName = item.ChargeName,
                        FundDetailId = item.FundDetailId.Value,
                        PreviousReading = item.PreviousReading.Value,
                        CurrentReading = item.CurrentReading.Value,
                        ConsumUnit = item.ConsumUnit.Value,
                        Amount = item.Amount.Value,
                        OrganizationId = User.OrgId,
                        Year = (short)Convert.ToDateTime(model.Date).Year,
                        Month = (short)Convert.ToDateTime(model.Date).Month,
                        ShopId = model.ShopId,
                        EntryUser = User.UserName,
                        EntryDate = DateTime.Now,
                        UnitRate = item.UnitRate.Value,
                        StateStatus = "Pending",
                        Remark = item.Remark,
                        AmountBN = amountBN,
                        BillExpireDate = month.Year.ToString() + "-" + month.Month.ToString() + "-" + LastDayOfMonth.ToString()
                    };
                    listMonthWiseShopCharge.Add(monthWiseShopCharge);
                }

                // Event Charge Checking //
                var floorId = db.tblShops.FirstOrDefault(s => s.ShopId == model.ShopId).FloorId;
                var eventData = (from ed in db.tblEventDetail
                                 join e in db.tblEventInfo on ed.EventInfoId equals e.EventInfoId
                                 where e.OrganizationId == User.OrgId && ed.FloorId == floorId && e.Month == month.Month && e.Year == month.Year
                                 select new VmEventInfo { Amount = e.Amount, FundInfoId = e.FundInfoId, FundDetailId = e.FundDetailId }).ToList();

                if (eventData.Count > 0)
                {
                    foreach (var item in eventData)
                    {
                        MonthWiseShopCharge monthWiseShopCharge = new MonthWiseShopCharge()
                        {
                            FundInfoId = item.FundInfoId,
                            FundDetailId = item.FundDetailId,
                            Amount = item.Amount,
                            ChargeName = "Event Charge",
                            PreviousReading = 0,
                            CurrentReading = 0,
                            ConsumUnit = 0,
                            OrganizationId = User.OrgId,
                            Year = (short)Convert.ToDateTime(model.Date).Year,
                            Month = (short)Convert.ToDateTime(model.Date).Month,
                            ShopId = model.ShopId,
                            EntryUser = User.UserName,
                            EntryDate = DateTime.Now,
                            UnitRate = 0,
                            StateStatus = "Pending",
                            Remark = "Event Charge",
                            AmountBN = Utility.ConvertEnNumToBnNum(item.Amount.ToString()),
                            BillExpireDate = month.Year.ToString() + "-" + month.Month.ToString() + "-" + LastDayOfMonth.ToString()
                        };
                        listMonthWiseShopCharge.Add(monthWiseShopCharge);
                    }
                }

                db.tblMonthWiseShopCharges.AddRange(listMonthWiseShopCharge);
                db.SaveChanges();
                return RedirectToAction("ShopPanel");
            }
            if (model.ChargeDetails == null)
            {
                ModelState.AddModelError("", "শপের চার্জ গুলো লোড করুন");
            }
            ViewBag.Shops = (from shop in db.tblShops
                             join floor in db.tblFloors on shop.FloorId equals floor.FloorId
                             where shop.OrganizationId == User.OrgId
                             orderby floor.FloorId
                             select new SelectListItem
                             {
                                 Value = shop.ShopId.ToString(),
                                 Text = floor.FloorNo + "- " + shop.ShopName
                             }).ToList();
            ViewBag.MonthAndYear = Utility.GetMonthAndYearListBN(1);
            return View("CreateShopMonthlyChargesView", model);
        }

        [HttpGet]
        public ActionResult CreateShopMonthlyChargesView()
        {
            var model = new VmShopMonthlyChargeInfo();
            ViewBag.Floors = db.tblFloors.Where(f => f.OrganizationId == User.OrgId).Select(f => new SelectListItem
            {
                Text = f.FloorNo,
                Value = f.FloorId.ToString()
            }).ToList();

            // Previous
            //ViewBag.Shops = (from shop in db.tblShops
            //                 join floor in db.tblFloors on shop.FloorId equals floor.FloorId
            //                 where shop.OrganizationId == User.OrgId
            //                 orderby floor.FloorId
            //                 select new SelectListItem
            //                 {
            //                     Value = shop.ShopId.ToString(),
            //                     Text = floor.FloorNo + "- " + shop.ShopName
            //                 }).ToList();
            ViewBag.MonthAndYear = Utility.GetMonthAndYearListBN(13);
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateShopBillView(int shopId, string date)
        {
            if (shopId > 0 && !string.IsNullOrEmpty(date))
            {
                DateTime dateTime = Convert.ToDateTime(date);
                var data = db.tblMonthWiseShopCharges.FirstOrDefault(s => s.ShopId == shopId && s.Month == dateTime.Month && s.Year == dateTime.Year);
                if (data == null)
                {
                    List<VmShopMonthlyChargeDetails> spData = db.Database.SqlQuery<VmShopMonthlyChargeDetails>(string.Format(@"Exec spShopMonthlyCharges {0},'{1}',{2}", shopId, date, User.OrgId)).ToList();
                    return PartialView("_CreateShopBillView", spData);
                }
            }
            return Content(" ইতিমধ্যে বিল এন্ট্রি করেছেন");
        }

        [HttpPost]
        public ActionResult GetShopListForCharging(string chargeDate, long? floorId, long? holdingNo)
        {
            var data = new List<VmShop>();
            try
            {
                string param = string.Empty;
                if (holdingNo > 0)
                {
                    param += string.Format(@" and sh.HoldingId={0}", holdingNo);
                }
                string query = string.Format(@"Select Distinct t1.ShopId,ShopName,ProprietorName,MobileNo,t1.FloorId,FloorNo 'FloorName',StateStatus,SUBSTRING(HoldingNo,0,Len(HoldingNo)) 'HoldingNo',t1.EntryUser,'{1}' as 'Month','{2}' as 'Year'
From (Select s.ShopId,s.ShopName,s.ProprietorName,s.MobileNo,s.FloorId,f.FloorNo,s.StateStatus,s.EntryUser,
Cast((Select h.HoldingName+',' From tblHoldings h
Inner Join tblShopHolding sh on h.HoldingId = sh.HoldingId
Where sh.ShopId =s.ShopId and sh.FloorId=s.FloorId
Order By h.FloorId For XML PATH('')) as nvarchar(200)) 'HoldingNo'
From tblShops s
Inner Join tblFloors f on s.FloorId = f.FloorId where s.OrganizationId={0} and f.FloorId={4}) t1 
Inner Join tblShopHolding sh on t1.ShopId = sh.ShopId
Where t1.ShopId Not IN (Select ShopId From tblMonthWiseShopCharges Where [Month]={1} and [Year]={2}) {3}", User.OrgId, Convert.ToDateTime(chargeDate).Month, Convert.ToDateTime(chargeDate).Year, param, floorId);

                data = db.Database.SqlQuery<VmShop>(query).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
            return PartialView(data);
        }

        [HttpPost]
        public ActionResult CreateShopBillView2(long shopId, string date)
        {
            if (shopId > 0 && !string.IsNullOrEmpty(date))
            {
                DateTime dateTime = Convert.ToDateTime(date);
                var data = db.tblMonthWiseShopCharges.FirstOrDefault(s => s.ShopId == shopId && s.Month == dateTime.Month && s.Year == dateTime.Year);
                if (data == null)
                {
                    List<VmShopMonthlyChargeDetails> spData = db.Database.SqlQuery<VmShopMonthlyChargeDetails>(string.Format(@"Exec spShopMonthlyCharges {0},'{1}',{2}", shopId, date, User.OrgId)).ToList();
                    ViewBag.ShopInfo = db.Database.SqlQuery<VmShop>(string.Format(@"Select * From vwShopList Where ShopId = {0}", shopId)).Single();
                    ViewBag.BillMonth = Utility.GetMonthNameBN(Convert.ToDateTime(date).Month) + ", " + Utility.ConvertEnNumToBnNum(Convert.ToDateTime(date).Year.ToString());
                    return PartialView("_CreateShopBillView2", spData);
                }
            }
            return Content(" ইতিমধ্যে বিল এন্ট্রি করেছেন");
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult ChargeBillingView(int shopId, string date)
        {
            if (shopId > 0 && !string.IsNullOrEmpty(date))
            {
                DateTime dateTime = Convert.ToDateTime(date);
                var data = db.tblMonthWiseShopCharges.FirstOrDefault(s => s.ShopId == shopId && s.Month == dateTime.Month && s.Year == dateTime.Year);
                if (data == null)
                {

                    List<VmChargeBillingView> spData = db.Database.SqlQuery<VmChargeBillingView>(string.Format(@"Select s.ShopId,s.ShopName,fi.FundInfoId,fi.FundNameBN,fd.FundDetailId,fd.FundDetailNameBN,sc.ChargeAmountEN, fd.IsFan From tblShops s
                    Inner Join tblShopCharges sc on(s.ShopId = sc.ShopId)
                    Inner Join tblFundInfo fi on sc.FundInfoId = fi.FundInfoId
                    Inner Join tblFundDetail fd on sc.FundDetailId = fd.FundDetailId
                    Where s.ShopId={0} and s.OrganizationId={1} and fi.FundNameBN =N'বিদ্যুৎ' 
                    and (fd.FundDetailNameBN = N'বিদ্যুৎ বিল আদায়' OR fd.IsFan = 'True')
                    Order By fd.IsFan", shopId, User.OrgId)).ToList();
                    return PartialView("ChargeBillingView", spData);
                }
            }
            return Content("");
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveShopMonthlyChargesNew(List<VmShopMonthlyChargeDetails> model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && model.Count() > 0)
            {
                DateTime month2 = Convert.ToDateTime(model.FirstOrDefault().ChargeMonth);
                DateTime month = Convert.ToDateTime(model.FirstOrDefault().ChargeMonth).AddMonths(1);

                var sId = model.FirstOrDefault().ShopId;
                int LastDayOfMonth = DateTime.DaysInMonth(month.Year, month.Month);
                List<MonthWiseShopCharge> listMonthWiseShopCharge = new List<MonthWiseShopCharge>();
                foreach (var item in model)
                {
                    //item.Amount= item.Remark == "Electricity" ? (item.ConsumUnit == null || item.ConsumUnit == 0 ? 300 : item.Amount) : item.Amount;
                    string amountBN = Utility.ConvertEnNumToBnNum(item.Amount.ToString());
                    MonthWiseShopCharge monthWiseShopCharge = new MonthWiseShopCharge
                    {
                        FundInfoId = item.FundInfoId.Value,
                        ChargeName = item.ChargeName,
                        FundDetailId = item.FundDetailId.Value,
                        PreviousReading = item.PreviousReading.Value,
                        CurrentReading = item.CurrentReading.Value,
                        ConsumUnit = item.ConsumUnit.Value,
                        Amount = item.Amount.Value,
                        OrganizationId = User.OrgId,
                        Year = (short)Convert.ToDateTime(item.ChargeMonth).Year,
                        Month = (short)Convert.ToDateTime(item.ChargeMonth).Month,
                        ShopId = item.ShopId.Value,
                        EntryUser = User.UserName,
                        EntryDate = DateTime.Now,
                        UnitRate = item.UnitRate.Value,
                        StateStatus = "Pending",
                        Remark = item.Remark,
                        AmountBN = amountBN,
                        BillExpireDate = month.Year.ToString() + "-" + month.Month.ToString() + "-" + LastDayOfMonth.ToString()
                    };
                    listMonthWiseShopCharge.Add(monthWiseShopCharge);
                }

                // Event Charge Checking //
                var floorId = db.tblShops.FirstOrDefault(s => s.ShopId == sId.Value).FloorId;
                var eventData = (from ed in db.tblEventDetail
                                 join e in db.tblEventInfo on ed.EventInfoId equals e.EventInfoId
                                 where e.OrganizationId == User.OrgId && ed.FloorId == floorId && e.Month == month2.Month && e.Year == month2.Year
                                 select new VmEventInfo { Amount = e.Amount, FundInfoId = e.FundInfoId, FundDetailId = e.FundDetailId, EventInfoId = e.EventInfoId }).ToList();

                if (eventData.Count > 0)
                {
                    foreach (var item in eventData)
                    {
                        MonthWiseShopCharge monthWiseShopCharge = new MonthWiseShopCharge()
                        {
                            FundInfoId = item.FundInfoId,
                            FundDetailId = item.FundDetailId,
                            Amount = item.Amount,
                            ChargeName = db.tblFundDetail.FirstOrDefault(f => f.FundDetailId == item.FundDetailId).FundDetailNameBN,
                            PreviousReading = 0,
                            CurrentReading = 0,
                            ConsumUnit = 0,
                            OrganizationId = User.OrgId,
                            Year = (short)month2.Year,
                            Month = (short)month2.Month,
                            ShopId = sId.Value,
                            EntryUser = User.UserName,
                            EntryDate = DateTime.Now,
                            UnitRate = 0,
                            StateStatus = "Pending",
                            Remark = "Others",
                            AmountBN = Utility.ConvertEnNumToBnNum(item.Amount.ToString()),
                            BillExpireDate = month.Year.ToString() + "-" + month.Month.ToString() + "-" + LastDayOfMonth.ToString(),
                            IsItEventCharge = true,
                            EventId = item.EventInfoId
                        };
                        listMonthWiseShopCharge.Add(monthWiseShopCharge);
                    }
                }
                db.tblMonthWiseShopCharges.AddRange(listMonthWiseShopCharge);
                IsSuccess = db.SaveChanges() > 0;
            }
            return Json(IsSuccess);
        }

        [HttpPost]
        public ActionResult FloorWiseShopMonthlyBillReport(long floorId, string date)
        {
            if (floorId > 0 && !string.IsNullOrEmpty(date))
            {
                List<ShopMonthlyBillInfo> shopMonthlyBillInfos = db.Database.SqlQuery<ShopMonthlyBillInfo>(string.Format(@"Select * FROM [dbo].[vw_ShopMonthlyBillInformation]  Where FloorId = {0} and OrganizationId = {1} and [Month]= {2} and [Year]={3}", floorId, User.OrgId, Convert.ToDateTime(date).Month, Convert.ToDateTime(date).Year)).ToList();
                LocalReport report = new LocalReport();
                string strReportPath = Server.MapPath("~/Reports/rptShopMonthlyBill_IM.rdlc");

                try
                {
                    if (System.IO.File.Exists(strReportPath) && shopMonthlyBillInfos.Count > 0)
                    {
                        NumberToBengaliWord bnTotalInWord = new NumberToBengaliWord();
                        foreach (var item in shopMonthlyBillInfos)
                        {
                            item.AmountInWord = bnTotalInWord.NumToWord(Math.Round(item.Amount).ToString());
                        }

                        report.ReportPath = strReportPath;
                        ReportDataSource reportDataSource = new ReportDataSource("BillInformation", shopMonthlyBillInfos);
                        report.DataSources.Clear();
                        report.DataSources.Add(reportDataSource);
                        report.SubreportProcessing += new SubreportProcessingEventHandler(BillDetail_SubreportProcessing);
                        report.Refresh();

                        string mimeType;
                        string encoding;
                        string fileNameExtension;

                        string deviceInfo =
                        "<DeviceInfo>" +
                        "<OutputFormat>PDF</OutputFormat>" +
                        "</DeviceInfo>";

                        Warning[] warnings;
                        string[] streams;
                        byte[] renderedBytes;

                        renderedBytes = report.Render(
                            "PDF",
                            deviceInfo,
                            out mimeType,
                            out encoding,
                            out fileNameExtension,
                            out streams,
                            out warnings);

                        var base64 = Convert.ToBase64String(renderedBytes);
                        var fs = String.Format("data:application/pdf;base64,{0}", base64);
                        return Json(new { isSuccess = true, report = fs, msg = "" });
                    }
                    return Json(new { isSuccess = false, report = "", msg = "File not found/ Data not found" });
                }
                catch (Exception ex)
                {

                }
            }
            return Json(new { isSuccess = false, report = "", msg = "One or more parameter is missing" });
        }

        //----//
        [HttpPost]
        public ActionResult GetShopBillDetails(long shopId, short month, short year)
        {
            if (shopId > 0 && month > 0 && year > 0)
            {
                ViewBag.shopName = db.tblShops.FirstOrDefault(s => s.ShopId == shopId).ShopName + "- " + Utility.GetMonthNameBN(month) + "," + Utility.ConvertEnNumToBnNum(year.ToString());

                var data = db.tblMonthWiseShopCharges.Include(m => m.Shop).Where(m => m.ShopId == shopId && m.Month == month && m.Year == year).ToList();

                ViewBag.ShopId = shopId;
                ViewBag.Month = month;
                ViewBag.Year = year;

                ViewBag.StateStatus = data.FirstOrDefault().StateStatus;
                return PartialView(data);
            }
            return Content("No Data Found");
        }

        [HttpPost]
        public ActionResult SaveBillStateStatus(long shopId, short month, short year)
        {
            bool isSuccess = false;
            if (shopId > 0 && month > 0 && year > 0)
            {
                var pendingChargeInDb = db.tblMonthWiseShopCharges.Where(p => p.ShopId == shopId && p.Month == month && p.Year == year && p.StateStatus == "Pending").ToList();
                if (pendingChargeInDb.Count > 0)
                {
                    foreach (var item in pendingChargeInDb)
                    {
                        item.StateStatus = "Paid";
                        item.UpdateBy = User.UserName;
                        item.UpdateDate = DateTime.Now;
                    }
                    isSuccess = db.SaveChanges() > 0;
                }
            }
            return Json(isSuccess);
        }

        [HttpPost]
        public ActionResult ShopMonthlyBillReport(long shopId, short month, short year)
        {
            if (shopId > 0 && month > 0 && year > 0)
            {
                List<ShopMonthlyBillInfo> shopMonthlyBillInfos = db.Database.SqlQuery<ShopMonthlyBillInfo>(string.Format(@"Select * FROM [dbo].[vw_ShopMonthlyBillInformation]  Where ShopId = {0} and OrganizationId = {1} and [Month]= {2} and [Year]={3}", shopId, User.OrgId, month, year)).ToList();
                LocalReport report = new LocalReport();
                string strReportPath = Server.MapPath("~/Reports/rptShopMonthlyBill_IM.rdlc");

                //try
                //{
                if (System.IO.File.Exists(strReportPath) && shopMonthlyBillInfos.Count > 0)
                {
                    NumberToBengaliWord bnTotalInWord = new NumberToBengaliWord();
                    foreach (var item in shopMonthlyBillInfos)
                    {
                        item.AmountInWord = bnTotalInWord.NumToWord(Math.Round(item.Amount).ToString());
                    }

                    report.ReportPath = strReportPath;
                    ReportDataSource reportDataSource = new ReportDataSource("BillInformation", shopMonthlyBillInfos);
                    report.DataSources.Clear();
                    report.DataSources.Add(reportDataSource);
                    report.SubreportProcessing += new SubreportProcessingEventHandler(BillDetail_SubreportProcessing);
                    report.Refresh();

                    string mimeType;
                    string encoding;
                    string fileNameExtension;

                    string deviceInfo =
                    "<DeviceInfo>" +
                    "<OutputFormat>PDF</OutputFormat>" +
                    "</DeviceInfo>";

                    Warning[] warnings;
                    string[] streams;
                    byte[] renderedBytes;

                    renderedBytes = report.Render(
                        "PDF",
                        deviceInfo,
                        out mimeType,
                        out encoding,
                        out fileNameExtension,
                        out streams,
                        out warnings);

                    var base64 = Convert.ToBase64String(renderedBytes);
                    var fs = String.Format("data:application/pdf;base64,{0}", base64);
                    return Json(new { isSuccess = true, report = fs, msg = "" });
                }
                return Json(new { isSuccess = false, report = "", msg = "File not found/ Data not found" });
                //}
                //catch (Exception ex)
                //{

                //}
            }
            return Json(new { isSuccess = false, report = "", msg = "One or more parameter is missing" });
        }

        [HttpGet]
        public virtual ActionResult DownloadPdf(string file)
        {
            if (!string.IsNullOrEmpty(file))
            {

                byte[] data = Encoding.ASCII.GetBytes(file);
                return File(data, "application/pdf", "File");
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult DeleteShopBill(long shopId, short month, short year)
        {
            bool IsSuccess = false;
            var data = db.tblMonthWiseShopCharges.Where(c => c.ShopId == shopId && c.Month == month && c.Year == year).ToList();
            if (data != null)
            {
                db.tblMonthWiseShopCharges.RemoveRange(data);
                IsSuccess = db.SaveChanges() > 0;
            }
            return Json(IsSuccess);
        }
        private string ShopParam(long? shopId, long? floorId, string date, string status, string holding)
        {
            string param = string.Empty;
            if (shopId != null && shopId > 0)
            {
                param += string.Format(@" And S.ShopId={0}", shopId);
            }
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" And f.floorId={0}", floorId);
            }
            if (!string.IsNullOrEmpty(date))
            {
                var d = Convert.ToDateTime(date);
                param += string.Format(@" And msc.[Month]={0} and msc.[Year]={1}", d.Month, d.Year);
            }
            if (!string.IsNullOrEmpty(holding))
            {
                param += string.Format(@" And h.HoldingName LIKE N'%{0}%'", holding);
            }
            if (!string.IsNullOrEmpty(status))
            {
                param += string.Format(@" And msc.StateStatus='{0}'", status);
            }
            param = Utility.ParamChecker(param);
            return param;
        }

        // Bill Details Sub Report...
        void BillDetail_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            List<ShopMonthlyBillDetail> lst_shopBillDetail = new List<ShopMonthlyBillDetail>();
            try
            {
                long ShopId = Convert.ToInt64(e.Parameters["ShopId"].Values[0]);
                short Month = Convert.ToInt16(e.Parameters["Month"].Values[0]);
                short Year = Convert.ToInt16(e.Parameters["Year"].Values[0]);
                string reportPath = e.ReportPath;

                if (reportPath == "rptShopMonthlyBillDetail_IM")
                {
                    lst_shopBillDetail = db.Database.SqlQuery<ShopMonthlyBillDetail>(string.Format(@"SELECT * FROM [dbo].[vw_ShopMonthlyBillDetail] Where ShopId = {0} and [Month]={1} and [Year] ={2}", ShopId, Month, Year)).ToList();
                    int count = 0;
                    foreach (var item in lst_shopBillDetail)
                    {
                        count++;
                        item.SLNo = Utility.ConvertEnNumToBnNum(count.ToString());
                    }
                    e.DataSources.Add(new ReportDataSource("BillDetails", lst_shopBillDetail));
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Revenue & Expense
        [HttpGet]
        public ActionResult RevenueExpensePanel()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetRevenueExpenseList(long? fundInfoId, long? fundDetailId, string fundType = "", string fromDate = "", string toDate = "")
        {
            List<VmRevenueExpenseDto> data = db.Database.SqlQuery<VmRevenueExpenseDto>(ParamRevEx(fundInfoId, fundDetailId, fundType, fromDate, toDate)).ToList();
            var r = data.Where(s => s.Type == "Revenue").Select(s => s.Amount).Sum();
            var e = data.Where(s => s.Type == "Expense").Select(s => s.Amount).Sum();
            ViewBag.Revenue = r > 0 ? Utility.ConvertEnNumToBnNum(r.ToString())  : "০.০০";
            ViewBag.Expense = e > 0 ? Utility.ConvertEnNumToBnNum(e.ToString()) : "০.০০";
            return View(data);
        }

        private string ParamRevEx(long? fundInfoId, long? fundDetailId, string fundType, string fromDate, string toDate)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (fundInfoId != null && fundInfoId > 0)
            {
                param += string.Format(@" And fi.FundInfoId={0}", fundInfoId);
            }
            if (fundDetailId != null && fundDetailId > 0)
            {
                param += string.Format(@" And fd.FundDetailId={0}", fundDetailId);
            }
            if (!string.IsNullOrEmpty(fundType))
            {
                param += string.Format(@" And fd.Type='{0}'", fundType);
            }
            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                param += string.Format(@" And Cast(re.BillDate as date) between '{0}' and '{1}' ", Convert.ToDateTime(fromDate).Date, Convert.ToDateTime(toDate).Date);
            }
            else if (!string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
            {
                param += string.Format(@" And Cast(re.BillDate as date)='{0}' ", Convert.ToDateTime(fromDate).Date);
            }
            else if (string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                param += string.Format(@" And Cast(re.BillDate as date)='{0}' ", Convert.ToDateTime(toDate).Date);
            }
            param = Utility.ParamChecker(param);

            query = string.Format(@"Select re.RevExId,re.RevExName,fi.FundInfoId,fi.FundNameBN,fd.FundDetailId,fd.FundDetailNameBN,fd.[Type],fd.TypeBN,
Cast((dbo.fnGetBnNumerical(Cast(DATEPART(DAY,re.BillDate) as nvarchar(100))) +' '+dbo.fnGetMonthNameBN(DATEPART(MONTH,re.BillDate)) +' '+dbo.fnGetBnNumerical(Cast(DATEPART(YEAR,re.BillDate) as nvarchar(100)))) as Nvarchar(20)) 'BillDate',re.Amount,re.AmountBN,re.PayFromOrPayTo,re.Remark
From tblRevenueExpenses re
Inner Join tblFundInfo fi on re.FundInfoId = fi.FundInfoId
Inner Join tblFundDetail fd on re.FundDetailId = fd.FundDetailId
where re.OrganizationId ={0} and re.IsOpeningBalance=0 {1} Order By Cast(BillDate as date)", User.OrgId, param);
            return query;
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRevenueExpense(List<VmRevenueExpense> model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && model.Count > 0)
            {
                try
                {
                    foreach (var item in model)
                    {
                        if (item.RevExId <= 0)
                        {
                            DateTime billdate = Convert.ToDateTime(item.BillDate);
                            DateTime thisDate = DateTime.Now;

                            string billNo = thisDate.Year.ToString().Substring(2) +
                                thisDate.Month.ToString().PadLeft(2, '0') + thisDate.ToString("dd") + thisDate.ToString("HH") + thisDate.ToString("mm") + thisDate.ToString("ss") + thisDate.ToString("ff");

                            RevenueExpense revenueExpense = new RevenueExpense
                            {
                                RevExName = item.RevExName,
                                FundInfoId = item.FundInfoId,
                                FundDetailId = item.FundDetailId,
                                Amount = item.Amount,
                                PayFromOrPayTo = item.PayFromOrPayTo,
                                Remark = item.Remark,
                                BillDate = billdate,
                                OrganizationId = User.OrgId,
                                AmountBN = Utility.ConvertEnNumToBnNum(item.Amount.ToString()),
                                StateStatus = "Paid",
                                EntryDate = thisDate,
                                EntryUser = User.UserName,
                                BillNo = billNo,
                                IsOpeningBalance = false
                            };
                            db.tblRevenueExpenses.Add(revenueExpense);
                        }
                        else
                        {
                            var reExInDb = db.tblRevenueExpenses.FirstOrDefault(rv => rv.RevExId == item.RevExId);
                            if (reExInDb != null)
                            {
                                reExInDb.PayFromOrPayTo = item.PayFromOrPayTo;
                                reExInDb.Remark = item.Remark;
                                reExInDb.Amount = item.Amount;
                                reExInDb.AmountBN = Utility.ConvertEnNumToBnNum(item.Amount.ToString());
                                reExInDb.BillDate = Convert.ToDateTime(item.BillDate).Date;
                            }
                        }
                    }
                    IsSuccess = db.SaveChanges() > 0;
                }
                catch (Exception ex)
                {

                }
            }
            return Json(IsSuccess);
        }

        [HttpGet]
        public ActionResult GetRevenueExpenseById(long id)
        {
            var data = db.tblRevenueExpenses.FirstOrDefault(rv => rv.RevExId == id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult DeleteRevenueExpenseById(long id)
        {
            bool IsSuccess = false;
            if (id > 0)
            {
                var data = db.tblRevenueExpenses.FirstOrDefault(rv => rv.RevExId == id);
                if (data != null)
                {
                    db.tblRevenueExpenses.Remove(data);
                    IsSuccess = db.SaveChanges() > 0;
                }
            }
            return Json(IsSuccess);
        }

        public ActionResult GetRevenueExpenseReport(long? FundInfo, long? FundDetail, string FundDetailType, string fromDate, string toDate)
        {
            List<VmRevenueExpenseDto> data = db.Database.SqlQuery<VmRevenueExpenseDto>(ParamRevEx(FundInfo, FundDetail, FundDetailType, fromDate, toDate)).ToList();
            string path = Server.MapPath("~/Reports/rptRevenueExpenseList.rdlc");

            if (System.IO.File.Exists(path))
            {
                LocalReport report = new LocalReport();
                report.ReportPath = path;
                report.DataSources.Clear();
                ReportDataSource reportDataSource = new ReportDataSource("RevenueExpenseList", data);
                report.DataSources.Clear();
                report.DataSources.Add(reportDataSource);
                report.Refresh();

                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =
                "<DeviceInfo>" +
                "<OutputFormat>PDF</OutputFormat>" +
                "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = report.Render(
                    "PDF",
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                return File(renderedBytes, mimeType);
            }
            return Content("Report Not Found");
        }
        #endregion

        #region Report Panel
        [HttpGet]
        public ActionResult ReportPanel()
        {
            ViewBag.ddlFundInfo = db.tblFundInfo.Where(f => f.OrganizationId == User.OrgId).Select(o => new SelectListItem
            {
                Text = o.FundNameBN,
                Value = o.FundInfoId.ToString()
            });
            return View();
        }

        [HttpGet]
        public ActionResult GetReportShow(long fundInfoId = 0, string fromDate = "", string toDate = "")
        {
            List<RevenueExpenseDetail> reportData = new List<RevenueExpenseDetail>();
            if (fundInfoId > 0 && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                var fdate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                var tdate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                string query = string.Empty;
                if(db.tblFundInfo.FirstOrDefault(f=> f.FundInfoId == fundInfoId).FundNameBN != "পার্কিং")
                {
                    query = string.Format(@"Exec spGetRevenueExpenseCal {0},'{1}','{2}',{3}", User.OrgId, fdate, tdate, fundInfoId);
                }
                else
                {
                    query = string.Format(@"Exec [spGetParkingRevenueExpenseCal] {0},'{1}','{2}',{3}", User.OrgId, fdate, tdate, fundInfoId);
                }
                reportData = db.Database.SqlQuery<RevenueExpenseDetail>(query).ToList();
            }
            return View(reportData);
        }

        [HttpPost]
        public ActionResult RevenueExpenseReport1(long fundInfoId = 0, string fromDate = "", string toDate = "")
        {
            bool IsSuccess = false;
            List<RevenueExpenseDetail> reportData = new List<RevenueExpenseDetail>();
            if (fundInfoId > 0 && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                var fdate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                var tdate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                string query = string.Empty;
                var fi = db.tblFundInfo.FirstOrDefault(f => f.FundInfoId == fundInfoId && f.OrganizationId == User.OrgId);
                if (fi == null)
                {
                    return Json(new { isSuccess = IsSuccess, report = "", msg = "No Found Found" });
                }
                if (db.tblFundInfo.FirstOrDefault(f => f.FundInfoId == fundInfoId).FundNameBN != "পার্কিং")
                {
                    query = string.Format(@"Exec spGetRevenueExpenseCal {0},'{1}','{2}',{3}", User.OrgId, fdate, tdate, fundInfoId);
                }
                else
                {
                    query = string.Format(@"Exec [spGetParkingRevenueExpenseCal] {0},'{1}','{2}',{3}", User.OrgId, fdate, tdate, fundInfoId);
                }
                reportData = db.Database.SqlQuery<RevenueExpenseDetail>(query).ToList();

                if (reportData.Count > 0)
                {
                    LocalReport report = new LocalReport();
                    string strReportPath = string.Empty;
                    if (fi.FundNameBN.Trim() == "পার্কিং")
                    {
                        strReportPath = Server.MapPath("~/Reports/rptRevenueExpense3.rdlc");
                    }
                    else if (fi.FundNameBN.Trim() != "পার্কিং")
                    {
                        strReportPath = Server.MapPath("~/Reports/rptRevenueExpense1.rdlc");
                    }
                    if (System.IO.File.Exists(strReportPath))
                    {
                        // Report Info Data //

                        List<RevenueExpenseInfo> info = new List<RevenueExpenseInfo>
                        {
                            new RevenueExpenseInfo
                            {
                                FundInfoNameBN = reportData.FirstOrDefault().FIName,
                                Organization = User.OrgName,
                                FromDateBN =Utility.ConvertEnNumToBnNum(Convert.ToDateTime(fromDate).Day.ToString())+"/"+Utility.ConvertEnNumToBnNum(Convert.ToDateTime(fromDate).Month.ToString())+"/"+Utility.ConvertEnNumToBnNum(Convert.ToDateTime(fromDate).Year.ToString()),
                                ToDateBN =Utility.ConvertEnNumToBnNum(Convert.ToDateTime(toDate).Day.ToString())+"/"+Utility.ConvertEnNumToBnNum(Convert.ToDateTime(toDate).Month.ToString())+"/"+Utility.ConvertEnNumToBnNum(Convert.ToDateTime(toDate).Year.ToString())
                            }
                        };
                        //------------------//

                        report.ReportPath = strReportPath;
                        ReportDataSource reportDataSource1 = new ReportDataSource("RevenueExpenseInfo", info);
                        ReportDataSource reportDataSource2 = new ReportDataSource("RevenueExpenseDetail", reportData);
                        report.DataSources.Clear();
                        report.DataSources.Add(reportDataSource1);
                        report.DataSources.Add(reportDataSource2);
                        report.Refresh();

                        string mimeType;
                        string encoding;
                        string fileNameExtension;

                        string deviceInfo =
                        "<DeviceInfo>" +
                        "<OutputFormat>PDF</OutputFormat>" +
                        "</DeviceInfo>";

                        Warning[] warnings;
                        string[] streams;
                        byte[] renderedBytes;

                        renderedBytes = report.Render(
                            "PDF",
                            deviceInfo,
                            out mimeType,
                            out encoding,
                            out fileNameExtension,
                            out streams,
                            out warnings);

                        IsSuccess = true;
                        var base64 = Convert.ToBase64String(renderedBytes);
                        var fs = String.Format("data:application/pdf;base64,{0}", base64);
                        return Json(new { isSuccess = true, report = fs, msg = "" });
                    }
                    return Json(new { isSuccess = IsSuccess, report = "", msg = "Report not found" });
                }
                return Json(new { isSuccess = IsSuccess, report = "", msg = "No Data Found" });
            }
            return Json(new { isSuccess = IsSuccess, report = "", msg = "Validation Error" });
        }

        public ActionResult ElectricityAndFanBillDetail(long floor=0,int month=0,int year = 0,string format="PDF")
        {
            var data = db.Database.SqlQuery<FloorAndShopWiseElectricityAndFanBillInfo>(string.Format(@"Exec spFloorAndShopWiseElectricityBillWithFan {0},{1},{2},{3}",month,year,User.OrgId,floor)).ToList();

            var funds= data.Select(s => new { FundDetailId = s.FundDetailId, FundDetailNameBN = s.FundDetailNameBN }).Distinct().ToList();
            List<FloorAndShopWiseElectricityAndFanBillSummery> summeries = new List<FloorAndShopWiseElectricityAndFanBillSummery>();
            FloorAndShopWiseElectricityAndFanBillSummery summery = new FloorAndShopWiseElectricityAndFanBillSummery();

            foreach (var item in funds)
            {
                if(item.FundDetailNameBN == "বিদ্যুৎ বিল")
                {
                    summery.ElectricityConsumeUnit = data.Where(s => s.FundDetailId == item.FundDetailId).Select(s => s.ConsumUnit).Sum();
                    summery.ElectricityConsumeUnitBN = Utility.ConvertEnNumToBnNum(summery.ElectricityConsumeUnit.ToString());
                    summery.ElectricityAmount = data.Where(s => s.FundDetailId == item.FundDetailId).Select(s => s.Amount).Sum();
                    summery.ElectricityAmountBN = Utility.ConvertEnNumToBnNum(summery.ElectricityAmount.ToString());
                }
                else if (item.FundDetailNameBN == "ফ্যান বিল")
                {
                    summery.FanConsumeUnit = data.Where(s => s.FundDetailId == item.FundDetailId).Select(s => s.ConsumUnit).Sum();
                    summery.FanConsumeUnitBN = Utility.ConvertEnNumToBnNum(summery.FanConsumeUnit.ToString());
                    summery.FanAmount = data.Where(s => s.FundDetailId == item.FundDetailId).Select(s => s.Amount).Sum();
                    summery.FanAmountBN = Utility.ConvertEnNumToBnNum(summery.FanAmount.ToString());
                }
                else
                {
                    summery.RoadLineConsumeUnit = data.Where(s => s.FundDetailId == item.FundDetailId).Select(s => s.ConsumUnit).Sum();
                    summery.RoadLineConsumeUnitBN = Utility.ConvertEnNumToBnNum(summery.RoadLineConsumeUnit.ToString());
                    summery.RoadLineAmount = data.Where(s => s.FundDetailId == item.FundDetailId).Select(s => s.Amount).Sum();
                    summery.RoadLineAmountBN = Utility.ConvertEnNumToBnNum(summery.RoadLineAmount.ToString());
                }
            }

            summery.TotalCurrentConsumeUnit = (summery.ElectricityConsumeUnit + summery.FanConsumeUnit+ summery.RoadLineConsumeUnit);
            summery.TotalCurrentConsumeUnitBN = Utility.ConvertEnNumToBnNum(summery.TotalCurrentConsumeUnit.ToString());
            summery.TotalAmount = (summery.ElectricityAmount + summery.FanAmount + summery.RoadLineAmount);
            summery.TotalAmountBN = Utility.ConvertEnNumToBnNum(summery.TotalAmount.ToString());
            summeries.Add(summery);

            string path = string.Format(@"~/Reports/rptFloorAndShopWiseConsumeElectricityDetail.rdlc");
            byte[] fileBytes = null;
            string fileMimeType = null;
            string rptType = format;

            GetReportFileByTwoDataSource(path, data, "FloorAndShopWiseElectricityAndFanBillInfo", summeries, "FloorAndShopWiseElectricityAndFanBillSummery", rptType, out fileBytes, out fileMimeType);

            var fileName = "ফ্লোর এবং শপ অনুযায়ী ব্যবহৃত কারেন্ট রিপোর্ট_" + DateTime.Now.ToString("dd-MMM-yyyy") + (format == "PDF" ? ".pdf" : (format == "EXCEL" ? ".xls" : ".doc"));
            if (format == "PDF")
            {
                return File(fileBytes, fileMimeType);
            }
            else
            {
                return File(fileBytes, fileMimeType, fileName);
            }
        }

        public ActionResult GetShopChargesDetail(string fromDate,string toDate, string format="PDF", long floorId=0)
        {
            DateTime f = new DateTime();
            DateTime t = new DateTime();
            DateTime.TryParse(fromDate, out f);
            DateTime.TryParse(toDate, out t);
            string query = string.Format(@"Exec [dbo].[spShopChargesDetail] {0},{1},{2},{3}", (fromDate == null || fromDate == string.Empty ? null : string.Format(@"'{0}'", new DateTime(f.Year,f.Month,01).ToString("yyyy-MM-dd"))), (toDate == null || toDate == string.Empty ? null : string.Format(@"'{0}'", new DateTime(t.Year, t.Month, 01).ToString("yyyy-MM-dd"))), User.OrgId, floorId);

            var data = db.Database.SqlQuery<MonthlyShopChargesDetail>(query).ToList();

            string path = string.Format(@"~/Reports/rptMonthlyShopChargesDetail.rdlc");
            byte[] fileBytes = null;
            string fileMimeType = null;
            string rptType = format;

            GetReportFileByOneDataSource(path, data, "MonthlyShopChargesDetail", rptType, out fileBytes, out fileMimeType);

            var fileName = "শপ চার্জ সমূহ" + (format == "PDF" ? ".pdf" : (format == "EXCEL" ? ".xls" : ".doc"));
            if (format == "PDF")
            {
                return File(fileBytes, fileMimeType);
            }
            else
            {
                return File(fileBytes, fileMimeType, fileName);
            }
        }

        public ActionResult GetInvoiceDetailsReport(long? floorId, long? holdingNo, long? shopId, string fromDate, string toDate,string format = "PDF" )
        {
            floorId = floorId == null ? 0 : floorId.Value;
            shopId = shopId == null ? 0 : shopId.Value;
            holdingNo = holdingNo == null ? 0 : holdingNo.Value;
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                fromDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                toDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                fromDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                toDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
            }
            var data = db.Database.SqlQuery<InvoiceDetailReport>(string.Format(@"Exec spInvoiceDetailReport {0},{1},{2},{3},'{4}','{5}'",User.OrgId, floorId,holdingNo,shopId,fromDate, toDate)).ToList();

            string path = string.Format(@"~/Reports/rptInvoiceDetailReport.rdlc");
            byte[] fileBytes = null;
            string fileMimeType = null;
            string rptType = format;

            GetReportFileByOneDataSource(path, data, "InvoiceDetailReport", rptType, out fileBytes, out fileMimeType);

            var fileName = "InvoiceDetailReport" + (format == "PDF" ? ".pdf" : (format == "EXCEL" ? ".xls" : ".doc"));
            if (format == "PDF")
            {
                return File(fileBytes, fileMimeType);
            }
            else
            {
                return File(fileBytes, fileMimeType, fileName);
            }
        }
        #endregion

        #region Dashboard
        [HttpPost]
        public ActionResult GetDashboardShopDueBills(long shopId)
        {
            List<VmDashBoardDueBillDetail> dueList = new List<VmDashBoardDueBillDetail>();
            try
            {
                if (shopId > 0)
                {
                    dueList = db.Database.SqlQuery<VmDashBoardDueBillDetail>(string.Format(@"Exec spDashBoardShopDueDetails {0}, {1}", User.OrgId, shopId)).ToList();

                    string query = string.Format(@"Select ShopId,ShopName,ProprietorName,MobileNo,FloorId,FloorNo 'FloorName',StateStatus,SUBSTRING(HoldingNo,0,Len(HoldingNo)) 'HoldingNo',EntryUser 
From (Select s.ShopId,s.ShopName,s.ProprietorName,s.MobileNo,s.FloorId,f.FloorNo,s.StateStatus,s.EntryUser,
Cast((Select h.HoldingName+',' From tblHoldings h
Inner Join tblShopHolding sh on h.HoldingId = sh.HoldingId
Where sh.ShopId =s.ShopId and sh.FloorId=s.FloorId
Order By h.FloorId For XML PATH('')) as nvarchar(200)) 'HoldingNo'
From tblShops s
left Join tblFloors f on s.FloorId = f.FloorId where s.OrganizationId={0} and s.ShopId={1}) t1
", User.OrgId, shopId);

                    ViewBag.ShopInfo = db.Database.SqlQuery<VmShop>(query).Single();
                    var totalAmount = dueList.Select(s => s.TotalAmount).Sum();
                    var totalFine = dueList.Select(s => s.Fine).Sum();
                    var totalGross = dueList.Select(s => s.GrossAmount).Sum();
                    var total = totalAmount + totalFine + totalGross;

                    ViewBag.TotalAmount = Utility.ConvertEnNumToBnNum(totalAmount.ToString());
                    ViewBag.TotalFine = Utility.ConvertEnNumToBnNum(totalFine.ToString());
                    ViewBag.TotalGross = Utility.ConvertEnNumToBnNum(totalGross.ToString());
                    ViewBag.Total = Utility.ConvertEnNumToBnNum(total.ToString());
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView(dueList);
        }

        [HttpPost]
        public ActionResult GetDashboardShopTodayFundDetailRevenue(long fundInfoId)
        {
            List<VmDashboardFundDetail> data = new List<VmDashboardFundDetail>();
            try
            {
                data = db.Database.SqlQuery<VmDashboardFundDetail>(string.Format(@"Select FundInfoId,FundNameBN,FundDetailId,FundDetailNameBN,SUM(Amount)'Amount', dbo.fnGetBnNumerical(Cast(SUM(Amount) as NVARCHAR(100))) 'AmountBN'
from  (Select fi.FundInfoId,fi.FundNameBN,fd.FundDetailId,fd.FundDetailNameBN,re.Amount
From tblRevenueExpenses re
Inner Join tblFundInfo fi on re.FundInfoId = fi.FundInfoId
Inner Join tblFundDetail fd on re.FundDetailId = fd.FundDetailId
where re.OrganizationId ={0} and re.IsOpeningBalance=0 and fd.[Type]='Revenue' 
And Cast(re.BillDate as Date) = Cast(GETDATE() as Date) and fi.FundInfoId={1}
Union All
Select fi.FundInfoId,fi.FundNameBN,fd.FundDetailId,fd.FundDetailNameBN,msc.Amount
From tblMonthWiseShopCharges msc
Inner Join tblFundInfo fi on msc.FundInfoId = fi.FundInfoId
Inner Join tblFundDetail fd on msc.FundDetailId = fd.FundDetailId
where msc.OrganizationId ={0} And Cast(msc.UpdateDate as Date) = Cast(GETDATE() as Date) and fd.[Type]='Revenue' 
and fi.FundInfoId={1} and msc.StateStatus='Paid'
) tbl
Group By FundInfoId,FundNameBN,FundDetailId,FundDetailNameBN", User.OrgId, fundInfoId)).ToList();

                ViewBag.TotalAmountEN = data.Select(s => s.Amount).Sum().ToString();
                ViewBag.TotalAmountBN = Utility.ConvertEnNumToBnNum((string)ViewBag.TotalAmountEN);
                ViewBag.FundInfoBN = data.Count() > 0 ? data.ElementAt(0).FundNameBN + " " + string.Format(@" বাবদ আজকের আয়") : "";
            }
            catch (Exception ex)
            {

            }
            return PartialView(data);
        }

        [HttpPost]
        public ActionResult GetDashboardShopThisMonthFundDetailRevenue(long fundInfoId)
        {
            List<VmDashboardFundDetail> data = new List<VmDashboardFundDetail>();
            try
            {
                data = db.Database.SqlQuery<VmDashboardFundDetail>(string.Format(@"Select FundInfoId,FundNameBN,FundDetailId,FundDetailNameBN,SUM(Amount)'Amount', dbo.fnGetBnNumerical(Cast(SUM(Amount) as NVARCHAR(100))) 'AmountBN'
from  (Select fi.FundInfoId,fi.FundNameBN,fd.FundDetailId,fd.FundDetailNameBN,re.Amount
From tblRevenueExpenses re
Inner Join tblFundInfo fi on re.FundInfoId = fi.FundInfoId
Inner Join tblFundDetail fd on re.FundDetailId = fd.FundDetailId
where re.OrganizationId ={0} and re.IsOpeningBalance=0 and fd.[Type]='Revenue' 
and MONTH(re.BillDate) = MONTH(GETDATE()) and YEAR(re.BillDate) = YEAR(GETDATE()) and fi.FundInfoId={1}
Union All
Select fi.FundInfoId,fi.FundNameBN,fd.FundDetailId,fd.FundDetailNameBN,msc.Amount
From tblMonthWiseShopCharges msc
Inner Join tblFundInfo fi on msc.FundInfoId = fi.FundInfoId
Inner Join tblFundDetail fd on msc.FundDetailId = fd.FundDetailId
where msc.OrganizationId ={0} and MONTH(msc.UpdateDate) = MONTH(GETDATE()) and YEAR(msc.UpdateDate) = YEAR(GETDATE()) and fd.[Type]='Revenue' 
and fi.FundInfoId={1}
) tbl
Group By FundInfoId,FundNameBN,FundDetailId,FundDetailNameBN", User.OrgId, fundInfoId)).ToList();

                ViewBag.TotalAmountEN = data.Select(s => s.Amount).Sum().ToString();
                ViewBag.TotalAmountBN = Utility.ConvertEnNumToBnNum((string)ViewBag.TotalAmountEN);
                ViewBag.FundInfoBN = data.Count() > 0 ? data.ElementAt(0).FundNameBN + " " + string.Format(@" বাবদ এই মাসের আয়") : "";
            }
            catch (Exception ex)
            {

            }
            return PartialView("GetDashboardShopTodayFundDetailRevenue", data);
        }
        #endregion

        #region Bank
        [HttpGet]
        public ActionResult BankPanel()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetBankList()
        {
            var data = db.tblBanks.Where(b => 1 == 1).ToList();
            return PartialView(data);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveBank(VmBank model)
        {
            bool IsSuccess = false;
            try
            {
                if (model.BankId <= 0)
                {
                    var thisDate = DateTime.Now;
                    Bank bank = new Bank
                    {
                        BankName = model.BankName,
                        BranchName = model.BranchName,
                        AccountNo = model.AccountNo,
                        IsActive = model.IsActive,
                        EntryUser = User.UserName,
                        EntryDate = thisDate,
                        OrganizationId = User.OrgId
                    };
                    db.tblBanks.Add(bank);
                }
                else
                {
                    var bankInBank = db.tblBanks.FirstOrDefault(b => b.BankId == model.BankId);
                    if (bankInBank != null)
                    {
                        bankInBank.AccountNo = model.AccountNo;
                        bankInBank.BranchName = model.BranchName;
                        bankInBank.IsActive = model.IsActive;
                        bankInBank.UpdateBy = User.UserName;
                        bankInBank.UpdateDate = DateTime.Now;
                    }
                }
                IsSuccess = db.SaveChanges() > 0;
            }
            catch (Exception ex)
            {

            }
            return Json(IsSuccess);
        }

        [HttpGet]
        public ActionResult GetCashBankTransferList(long? fundId, long? bankId, string statement, double? amount, string flag)
        {
            var data = new List<VmCashToBankAndBankToCash>();
            try
            {
                string query = string.Format(@"Select Id,BankToCash,CashToBank,Remark,EntryUser,FundInfoId,FundNameBN,BankId,BankName,AccountNo,dbo.fnGetBnNumerical(Cast(Day(BankDate1) as Nvarchar(10))) +' '+dbo.fnGetMonthNameBN(Month(BankDate1))
+' '+dbo.fnGetBnNumerical(Cast(Year(BankDate1) as Nvarchar(10))) 'BankDate', ISNULL(ChequeNo,'') 'ChequeNo'
 From(Select cb.Id,ISNULL(cb.BankToCash,0.00)'BankToCash',ISNULL(cb.CashToBank,0.00)'CashToBank',
cb.Remark,Convert(Nvarchar(15),Cast(BankDate as Date),106) 'BankDate1',
cb.EntryUser,f.FundInfoId,f.FundNameBN,b.BankId,b.BankName,b.AccountNo,cb.ChequeNo
From tblCashToBankAndBankToCashes cb
Inner Join tblBanks b on cb.BankId = b.BankId
Inner Join tblFundInfo f on cb.FundInfoId = f.FundInfoId
Where cb.OrganizationId={0} and cb.IsOpeningBalance=0) tbl Where 1=1", User.OrgId);
                if (string.IsNullOrEmpty(flag))
                {
                    data = db.Database.SqlQuery<VmCashToBankAndBankToCash>(query).ToList();
                }
                else
                {
                    query = query + paramGetCashBankTransferList(fundId, bankId, statement, amount);
                    data = db.Database.SqlQuery<VmCashToBankAndBankToCash>(query).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView(data);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveCashBankTransfer(VmCashToBankAndBankToCash model)
        {
            bool IsSuccess = false;
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id <= 0)
                    {
                        CashToBankAndBankToCash bankCash = new CashToBankAndBankToCash()
                        {
                            BankId = model.BankId,
                            BankDate = Convert.ToDateTime(model.BankDate).Date.ToString("yyyy-MM-dd"),
                            OrganizationId = User.OrgId,
                            Remark = model.Remark,
                            FundInfoId = model.FundInfoId,
                            BankToCash = model.BankToCash,
                            CashToBank = model.CashToBank,
                            EntryUser = User.UserName,
                            EntryDate = DateTime.Now,
                            ChequeNo = ((model.BankToCash != null && model.BankToCash > 0) ? model.ChequeNo : null)
                        };
                        db.tblCashToBankAndBankToCashes.Add(bankCash);
                    }
                    else
                    {
                        var dataInDb = db.tblCashToBankAndBankToCashes.FirstOrDefault(cb => cb.Id == model.Id);
                        if (dataInDb != null)
                        {
                            dataInDb.BankId = model.BankId;
                            dataInDb.FundInfoId = model.FundInfoId;
                            dataInDb.BankToCash = model.BankToCash;
                            dataInDb.CashToBank = model.CashToBank;
                            dataInDb.ChequeNo = ((model.BankToCash != null && model.BankToCash > 0) ? model.ChequeNo : null);

                            dataInDb.UpdateBy = User.UserName;
                            dataInDb.UpdateDate = DateTime.Now;
                        }
                    }
                    IsSuccess = db.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
            }

            return Json(IsSuccess);
        }

        [HttpGet]
        public ActionResult GetCashBankTransferById(long id)
        {
            var cb = db.tblCashToBankAndBankToCashes.FirstOrDefault(c => c.Id == id);
            var data = new
            {
                Id = cb.Id,
                BankId = cb.BankId,
                Amount = ((cb.BankToCash.HasValue && cb.BankToCash.Value > 0) ? cb.BankToCash.Value : cb.CashToBank.Value),
                FundId = cb.FundInfoId,
                IsBank2Cash = ((cb.BankToCash.HasValue && cb.BankToCash.Value > 0) ? 2 : 1),
                Remark = cb.Remark
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetFundBankOBList()
        {
            var data = new List<VmFundBankOB>();
            try
            {
                data = db.Database.SqlQuery<VmFundBankOB>(string.Format(@"Select cb.Id,b.BankId,(b.BankName+' ('+b.AccountNo+')') 'BankName',fi.FundInfoId,fi.FundNameBN,cb.CashToBank 'Amount',dbo.fnGetBnNumerical(Cast(cb.CashToBank as Nvarchar(20))) 'AmountBN',cb.BankDate 'BankDateEN',
(dbo.fnGetBnNumerical(Cast(DAY(Cast(BankDate as date)) as Nvarchar(5))) +' '+dbo.fnGetMonthNameBN(Month(Cast(BankDate as date)))+' '+dbo.fnGetBnNumerical(Cast(YEAR(Cast(BankDate as date)) as Nvarchar(5)))) 'BankDateBN'
From tblCashToBankAndBankToCashes cb
Inner Join tblBanks b on cb.BankId = b.BankId
Inner Join tblFundInfo fi on cb.FundInfoId = fi.FundInfoId
Where cb.IsOpeningBalance = 'True' and cb.OrganizationId={0} ", User.OrgId)).ToList();

            }
            catch (Exception ex)
            {

            }
            return PartialView(data);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveFundBankOB(VmFundBankOB model)
        {
            bool IsSuccess = false;
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id <= 0)
                    {
                        CashToBankAndBankToCash domain = new CashToBankAndBankToCash
                        {
                            BankId = model.BankId,
                            FundInfoId = model.FundInfoId,
                            CashToBank = model.Amount,
                            IsOpeningBalance = true,
                            EntryUser = User.UserName,
                            EntryDate = DateTime.Now,
                            BankDate = Convert.ToDateTime(model.BankDateEN).Date.ToString("yyyy-MM-dd"),
                            OrganizationId = User.OrgId
                        };
                        db.tblCashToBankAndBankToCashes.Add(domain);
                    }
                    else
                    {
                        var dataInDb = db.tblCashToBankAndBankToCashes.FirstOrDefault(cb => cb.Id == model.Id);
                        if (dataInDb != null)
                        {
                            dataInDb.CashToBank = model.Amount > 0 ? model.Amount : dataInDb.CashToBank;
                        }
                    }
                    IsSuccess = db.SaveChanges() > 0;
                }
                //else
                //{
                //    foreach (ModelState modelState in ViewData.ModelState.Values)
                //    {
                //        foreach (ModelError error in modelState.Errors)
                //        {

                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                IsSuccess = false;
            }
            return Json(IsSuccess);
        }

        #endregion

        #region Params [No Action]
        [NonAction]
        private string paramGetCashBankTransferList(long? fundId, long? bankId, string statement, double? amount)
        {
            string param = string.Empty;
            if (fundId != null && fundId > 0)
            {
                param += string.Format(@" and FundInfoId={0}", fundId);
            }
            if (bankId != null && bankId > 0)
            {
                param += string.Format(@" and BankId={0}", bankId);
            }
            if (!string.IsNullOrEmpty(statement) && statement.Trim() != "")
            {
                if (statement == "1")
                {
                    param += string.Format(@" and CashToBank  > 0 and (BankToCash is null or BankToCash=0) ");
                }
                else if (statement == "2")
                {
                    param += string.Format(@" and BankToCash  > 0 and (CashToBank is null or CashToBank=0) ");
                }
            }
            if (amount != null && amount > 0)
            {
                if (!string.IsNullOrEmpty(statement) && statement.Trim() != "")
                {
                    if (statement == "1")
                    {
                        param += string.Format(@" and CashToBank={0} ", amount);
                    }
                    else if (statement == "2")
                    {
                        param += string.Format(@" and BankToCash={0} ", amount);
                    }
                }
                else
                {
                    param += string.Format(@" and (ISNULL(BankToCash,0)={0} or ISNULL(CashToBank,0)={0})", amount);
                }
            }

            param = Checker.ElementValue(param);
            return param;
        }
        #endregion

        #region [Montly Bill Invoice]
        [HttpGet]
        public ActionResult GetInvoiceList(string flag, long? floorId, string holdingNo, long? shopId,string fromDate,string todate)
        {
            string query = string.Format(@"Select Distinct Inv.Id,Inv.InvoiceNo,F.FloorNo,dbo.fnGetShopHoldings(s.ShopId,f.FloorId,s.OrganizationId) 'HoldingNo', 
S.ShopId,S.ShopName,Inv.TotalAmount,Inv.TotalChargeAmount,Inv.TotalFineAmount,Inv.TotalConnectionFee,Inv.TotalAmountBN,Inv.TotalChargeAmountBN,Inv.TotalFineAmountBN,Inv.TotalConnectionFeeBN,Inv.TotalChargeAmountBN,Inv.EntryUser,Inv.UpdateBy,Inv.EntryDate,Inv.UpdateDate
From tblInvoiceInfo Inv
Inner Join tblFloors F on Inv.FloorId = F.FloorId
Inner Join tblShops S on Inv.ShopId = S.ShopId
Inner Join tblShopHolding sh on S.ShopId = sh.ShopId
Inner Join tblHoldings h on sh.HoldingId = h.HoldingId
Where Inv.OrganizationId = {0} {1}", User.OrgId, ParamGetInvoiceList(floorId, holdingNo, shopId, fromDate,todate));
            var data = db.Database.SqlQuery<VmInvoiceInfo>(query).ToList();

            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.FloorList = db.tblFloors.Where(f => f.OrganizationId == User.OrgId).Select(s => new SelectListItem
                {
                    Text = s.FloorNo.ToString(),
                    Value = s.FloorId.ToString()
                });

                return View(data);
            }
            else
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetInvoiceDetails(string invoiceNo)
        {
            string query = string.Format(@"Select dbo.fnGetBnNumerical(CAST(invI.InvoiceNo as nvarchar(50))) 'InvoiceNo',
(dbo.fnGetBnNumerical(Cast(DAY(invI.EntryDate) as nvarchar(4))) +'/'+dbo.fnGetBnNumerical(Cast(MONTH(invI.EntryDate) as nvarchar(4)))+'/'+dbo.fnGetBnNumerical(Cast(YEAR(invI.EntryDate) as nvarchar(4)))) 'InvoiceDate',s.ShopName
,(f.FloorNo +'/'+dbo.fnGetShopHoldings(s.ShopId,f.FloorId,invI.OrganizationId)) 'Address',
invI.TotalAmount,invI.TotalAmountBN,invI.TotalChargeAmount,invI.TotalChargeAmountBN,invI.TotalConnectionFee,
invI.TotalConnectionFeeBN,invI.TotalFineAmount,invI.TotalFineAmountBN,
(dbo.fnGetMonthNameBN(invD.[Month]) +','+ dbo.fnGetBnNumerical(Cast(invD.[Year] as nvarchar(4)))) 'MonthAndYear',
invD.ChargeAmount,invD.ChargeAmountBN,invD.FineAmount,invD.FineAmountBN,invD.ConnectionFee,invD.ConnectionFeeBN,invD.NetAmount,
invD.NetAmountBN From tblInvoiceDetails invD
Inner Join tblInvoiceInfo invI on invI.InvoiceNo = invD.InvoiceNo
Inner Join tblShops s on invI.ShopId = s.ShopId
Inner Join tblFloors f on s.FloorId = f.FloorId
Where invI.InvoiceNo='{0}'", invoiceNo);
            var data = db.Database.SqlQuery<VmInvoiceDetailForModal>(query).ToList();
            return Json(data);
        }

        [NonAction]
        private string ParamGetInvoiceList(long? floorId, string holdingNo, long? shopId,string fromDate,string toDate)
        {
            string param = string.Empty;
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" And F.FloorId ={0}", floorId);
            }
            if (!string.IsNullOrEmpty(holdingNo) && holdingNo !="0")
            {
                param += string.Format(@" And h.HoldingId ={0}", holdingNo);
            }
            if (shopId != null && shopId > 0)
            {
                param += string.Format(@" And S.ShopId  = {0}", shopId);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(Inv.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(Inv.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(Inv.EntryDate as date)='{0}'", tDate);
            }
            return param;
        }

        [HttpGet]
        public ActionResult CreateInvoice()
        {
            ViewBag.FloorList = db.tblFloors.Where(f => f.OrganizationId == User.OrgId).Select(s => new SelectListItem
            {
                Text = s.FloorNo.ToString(),
                Value = s.FloorId.ToString()
            });
            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveInvoice(VmInvoiceInfo info, List<VmInvoiceDetail> details)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && details.Count > 0)
            {
                var date = DateTime.Now;
                string inv = User.OrgId + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");

                List<InvoiceDetail> invoiceDetails = new List<InvoiceDetail>();
                foreach (var item in details)
                {
                    InvoiceDetail invoiceDetail = new InvoiceDetail
                    {
                        InvoiceNo = inv,
                        ShopId = item.ShopId,
                        Month = item.Month,
                        Year = item.Year,
                        FineAmount = item.FineAmount,
                        FineAmountBN = item.FineAmountBN,
                        ChargeAmount = item.ChargeAmount,
                        ChargeAmountBN = item.ChargeAmountBN,
                        ConnectionFee = item.ConnectionFee,
                        ConnectionFeeBN = item.ConnectionFeeBN,
                        NetAmount = item.NetAmount,
                        NetAmountBN = item.NetAmountBN,
                        EntryUser = User.UserName,
                        EntryDate = DateTime.Now,
                        OrganizationId = User.OrgId
                    };
                    invoiceDetails.Add(invoiceDetail);
                }
                
                InvoiceInfo invoice = new InvoiceInfo
                {
                    InvoiceNo = inv,
                    ShopId = info.ShopId,
                    TotalFineAmount = info.TotalFineAmount,
                    TotalFineAmountBN = info.TotalFineAmountBN,
                    TotalChargeAmount = info.TotalChargeAmount,
                    TotalChargeAmountBN = info.TotalChargeAmountBN,
                    TotalConnectionFee = info.TotalConnectionFee,
                    TotalConnectionFeeBN = info.TotalConnectionFeeBN,
                    TotalAmount = info.TotalAmount,
                    TotalAmountBN = info.TotalAmountBN,
                    OrganizationId = User.OrgId,
                    FloorId = info.FloorId,
                    HoldingId = info.HoldingId,
                    EntryUser = User.UserName,
                    EntryDate = DateTime.Now,
                    InvoiceDetails = invoiceDetails
                };

                short month = details.FirstOrDefault().Month;
                short year = details.FirstOrDefault().Year;

                var months = details.Select(d => d.Month).Distinct().ToList();
                var years = details.Select(d => d.Year).Distinct().ToList();

                var chargeData = db.tblMonthWiseShopCharges.Where(ms => ms.ShopId == info.ShopId && months.Contains(ms.Month) && years.Contains(ms.Year) && ms.StateStatus=="Pending").ToList();
                foreach (var item in chargeData)
                {
                    item.StateStatus = "Paid";
                    item.UpdateBy = User.UserName;
                    item.UpdateDate = DateTime.Now;
                    item.InvoiceNo = inv;
                }
                db.tblInvoiceInfo.Add(invoice);

                // Fine Part/ Connection Fee
                if (info.TotalFineAmount > 0)
                {
                    List<MonthWiseShopCharge> listFineData = new List<MonthWiseShopCharge>();
                    foreach (var item in details)
                    {
                        if (item.FineAmount > 0)
                        {
                            VmFineAndConn data = db.Database.SqlQuery<VmFineAndConn>(string.Format(@"Select f.FundInfoId,FineId=(Select FundDetailId From tblFundDetail Where FundInfoId = f.FundInfoId And FundDetailNameBN=N'সারচার্জ আদায়'),ConFeeId=(Select FundDetailId From tblFundDetail Where FundInfoId = f.FundInfoId And FundDetailNameBN=N'পুনঃ সংযোগ ফি') From tblFundInfo f Where FundNameBN = N'বিদ্যুৎ ফান্ড' And OrganizationId ={0}", User.OrgId)).Single();
                            var rowCount = (item.FineAmount > 0 && item.ConnectionFee > 0) ? 2 : 1;
                            for (int i = 0; i < rowCount; i++)
                            {
                                MonthWiseShopCharge shopCharge = new MonthWiseShopCharge
                                {
                                    ChargeName = ((i == 0) ? "সারচার্জ আদায়" : "পুনঃ সংযোগ ফি"),
                                    PreviousReading = 0,
                                    CurrentReading = 0,
                                    ConsumUnit = 0,
                                    UnitRate = ((i == 0) ? item.FineAmount.Value : item.ConnectionFee.Value),
                                    Amount = ((i == 0) ? item.FineAmount.Value : item.ConnectionFee.Value),
                                    FundInfoId = data.FundInfoId,
                                    FundDetailId = ((i == 0) ? data.FineId : data.ConFeeId),
                                    OrganizationId = User.OrgId,
                                    ShopId = info.ShopId,
                                    Month = item.Month,
                                    Year = item.Year,
                                    EntryUser = User.UserName,
                                    EntryDate = DateTime.Now,
                                    UpdateBy = User.UserName,
                                    UpdateDate = DateTime.Now,
                                    Remark = "Fine",
                                    AmountBN = ((i == 0) ? Utility.ConvertEnNumToBnNum(item.FineAmount.Value.ToString()) : Utility.ConvertEnNumToBnNum(item.ConnectionFee.Value.ToString())),
                                    StateStatus = "Paid",
                                    InvoiceNo = inv
                                };
                                listFineData.Add(shopCharge);
                            }
                        }
                    }

                    db.tblMonthWiseShopCharges.AddRange(listFineData);
                }

                IsSuccess = db.SaveChanges() > 0;

                if (IsSuccess)
                {
                    string base64 = InvoiceReport(inv);
                    if (string.IsNullOrEmpty(base64))
                    {
                        return Json(new { isSuccess = false, report = "", msg = "Report Not found / No Data Found" });
                    }
                    else
                    {
                        var fs = String.Format("data:application/pdf;base64,{0}", base64);
                        return Json(new { isSuccess = true, report = fs, msg = "" });
                    }
                }
            }
            return Json(new { isSuccess = IsSuccess, report = "", msg = "" });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetBillAmountData(long floorId, long holdingId, string key)
        {
            VmBillAmount billAmount = new VmBillAmount();
            if (floorId > 0 && holdingId > 0 && !string.IsNullOrEmpty(key) && key.Trim() != "")
            {
                string[] keyValue = key.Split(':');
                var date = Convert.ToDateTime(keyValue[1]);
                try
                {
                    billAmount = db.Database.SqlQuery<VmBillAmount>(string.Format(@"Exec spGetShopBillDetailByMonth {0},{1},{2},{3}", User.OrgId, Convert.ToInt64(keyValue[0]), ((short)date.Month), ((short)date.Year))).Single();
                }
                catch (Exception ex)
                {

                }
            }
            return Json(billAmount);
        }

        public ActionResult GetInvoiceReport(string invoiceNo)
        {
            string rptBase = string.Empty;
            if (!string.IsNullOrEmpty(invoiceNo))
            {
                rptBase = String.Format("data:application/pdf;base64,{0}", InvoiceReport(invoiceNo));
            }
            return Json(new {report= rptBase });
        }

        [NonAction]
        private string InvoiceReport(string InvoiceNo)
        {

            string base64 = string.Empty;
            try
            {
                List<ReportInvoiceInfo> rptInfo = db.Database.SqlQuery<ReportInvoiceInfo>(string.Format(@"Select inv.InvoiceNo,dbo.fnGetBnNumerical(CAST(inv.InvoiceNo as nvarchar(30))) 'InvoiceNoBN',s.ShopId,s.ShopName,
(f.FloorNo+'/'+[dbo].[fnGetShopHoldings](s.ShopId,f.FloorId,s.OrganizationId)) 'Address',
s.ProprietorName, (dbo.fnGetBnNumerical(Cast(DAY(inv.EntryDate) as nvarchar(100)))+'/'+dbo.fnGetBnNumerical(Cast(MONTH(inv.EntryDate) as nvarchar(100))) +'/'+dbo.fnGetBnNumerical(Cast(YEAR(inv.EntryDate) as nvarchar(100)))) 'InvoiceDate',
inv.TotalFineAmount,inv.TotalFineAmountBN,inv.TotalConnectionFee,inv.TotalConnectionFeeBN,inv.TotalChargeAmount,inv.TotalChargeAmountBN,inv.TotalAmount,inv.TotalAmountBN,'' TotalInWord 
From tblInvoiceInfo inv
Inner join tblShops s on inv.ShopId = s.ShopId
Inner Join tblFloors f on s.FloorId = f.FloorId
Where inv.InvoiceNo='{0}'", InvoiceNo)).ToList();

                List<ReportInvoiceDetail> rptDetails = db.Database.SqlQuery<ReportInvoiceDetail>(string.Format(@"Select dbo.fnGetBnNumerical(Cast(Serial as nvarchar(10))) 'Serial',MonthAndYear,ChargeAmount,ChargeAmountBN,ConnectionFee,ConnectionFeeBN,FineAmount,FineAmountBN,NetAmount,NetAmountBN,CurrentReading,dbo.fnGetBnNumerical(CAST(CurrentReading as nvarchar(50))) 'CurrentReadingBN',
PreviousReading, dbo.fnGetBnNumerical(CAST(PreviousReading as Nvarchar(50))) 'PreviousReadingBN'
 From (

Select ROW_NUMBER() Over(Order By [Month],[Year]) 'Serial',(dbo.fnGetMonthNameBN([Month]) +','+dbo.fnGetBnNumerical(CAST([Year] As nvarchar(4)))) 'MonthAndYear',d.ChargeAmount,d.ChargeAmountBN,d.ConnectionFee,d.ConnectionFeeBN,d.FineAmount,d.FineAmountBN,d.NetAmount,
d.NetAmountBN,

(
	Select CurrentReading From tblMonthWiseShopCharges msc 
	Inner Join tblFundDetail fd on msc.FundDetailId =fd.FundDetailId 
	Where ShopId=d.ShopId and [Month]=d.[Month] and [Year]=d.[Year] 
	and msc.OrganizationId =d.OrganizationId and fd.FundDetailNameBN=N'বিদ্যুৎ বিল আদায়'
)  as 'CurrentReading'
,
(
	Select PreviousReading From tblMonthWiseShopCharges msc 
	Inner Join tblFundDetail fd on msc.FundDetailId =fd.FundDetailId 
	Where ShopId=d.ShopId and [Month]=d.[Month] and [Year]=d.[Year] 
	and msc.OrganizationId =d.OrganizationId and fd.FundDetailNameBN=N'বিদ্যুৎ বিল আদায়'
)  as 'PreviousReading'

From tblInvoiceDetails d
Inner Join tblInvoiceInfo i on d.InvoiceNo = i.InvoiceNo
Where i.InvoiceNo='{0}'

) tbl
", InvoiceNo)).ToList();

                if (rptInfo.Count > 0 && rptDetails.Count > 0)
                {
                    LocalReport report = new LocalReport();
                    string reportPath = Server.MapPath("~/Reports/rptInvoice_IM.rdlc");
                    var amt = rptInfo.FirstOrDefault().TotalAmount;
                    NumberToBengaliWord bengaliWord = new NumberToBengaliWord();
                    var inWord = bengaliWord.NumToWord(Math.Round(amt).ToString());

                    rptInfo.FirstOrDefault().TotalInWord = inWord;

                    if (System.IO.File.Exists(reportPath))
                    {
                        ReportDataSource reportSourceInfo = new ReportDataSource("InvoiceInfo", rptInfo);
                        ReportDataSource reportSourceDetails = new ReportDataSource("InvoiceDetails", rptDetails);
                        report.ReportPath = reportPath;
                        report.DataSources.Clear();
                        report.DataSources.Add(reportSourceInfo);
                        report.DataSources.Add(reportSourceDetails);
                        report.Refresh();

                        string mimeType;
                        string encoding;
                        string fileNameExtension;

                        string deviceInfo =
                        "<DeviceInfo>" +
                        "<OutputFormat>PDF</OutputFormat>" +
                        "</DeviceInfo>";

                        Warning[] warnings;
                        string[] streams;
                        byte[] renderedBytes;

                        renderedBytes = report.Render(
                            "PDF",
                            deviceInfo,
                            out mimeType,
                            out encoding,
                            out fileNameExtension,
                            out streams,
                            out warnings);

                        base64 = Convert.ToBase64String(renderedBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                base64 = string.Empty;
            }

            return base64;
        }

        #endregion

        #region Event Charge
        [HttpGet]
        public ActionResult GetEventList()
        {
            ViewBag.MonthAndYear = Utility.GetMonthAndYearListBN(11);
            ViewBag.MonthAndYear2 = Utility.GetMonthAndYearListBN(11); //Utility.GetMonthAndYearListNextBN(11);
            ViewBag.ddlFundInfo = db.tblFundInfo.Where(fi => fi.OrganizationId == User.OrgId).Select(fi => new SelectListItem
            {
                Text = fi.FundNameBN,
                Value = fi.FundInfoId.ToString()
            }).ToList();

            ViewBag.ddlFloor = db.tblFloors.Where(f => f.OrganizationId == User.OrgId).Select(f => new SelectListItem { Text = f.FloorNo, Value = f.FloorId.ToString() }).ToList();

            var data = db.Database.SqlQuery<VmEventList>(string.Format(@"Select Distinct EventInfoId,EventName,MonthAndYear,SUBSTRING(EventFloors,0,LEN(EventFloors)) 'EventFloors',FundNameBN,FundDetailNameBN,Remarks,EntryUser,FloorId,Amount,AmountBN  
From (Select ei.EventInfoId,ei.EventName,(dbo.fnGetMonthNameBN([Month])+','+dbo.fnGetBnNumerical(Cast([YEAR] as nvarchar(20)))) 'MonthAndYear'
,Cast((Select f.FloorNo+',' From tblEventDetail e Inner Join tblFloors f on e.FloorId = f.FloorId
where e.EventInfoId = ei.EventInfoId
Order By f.FloorId For XML PATH('')) as Nvarchar(200)) 'EventFloors',fi.FundNameBN,fd.FundDetailNameBN,ISNULL(ei.Remarks,'') 'Remarks',ei.EntryUser,Cast('0' as bigint) 'FloorId',ei.Amount,ei.AmountBN 
From tblEventInfo ei
Inner Join tblEventDetail ed on ei.EventInfoId = ed.EventInfoId
Inner Join tblFundInfo fi on ei.FundInfoId = fi.FundInfoId
Inner Join tblFundDetail fd on ei.FundDetailId = fd.FundDetailId
Where ei.OrganizationId={0}) tbl", User.OrgId)).ToList();
            return View(data);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveEvent(VmEventInfo viewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid && viewModel.EventDetails.Count > 0)
            {
                var eventMonth = Convert.ToDateTime(viewModel.EventMonth);
                if (viewModel.EventInfoId == 0)
                {
                    EventInfo eventInfo = new EventInfo();
                    eventInfo.EventName = viewModel.EventName;
                    eventInfo.Month = (short)eventMonth.Month;
                    eventInfo.Year = (short)eventMonth.Year;
                    eventInfo.Amount = viewModel.Amount;
                    eventInfo.AmountBN = Utility.ConvertEnNumToBnNum(viewModel.Amount.ToString());
                    eventInfo.Remarks = viewModel.Remarks;
                    eventInfo.FundInfoId = viewModel.FundInfoId;
                    eventInfo.FundDetailId = viewModel.FundDetailId;
                    eventInfo.OrganizationId = User.OrgId;
                    eventInfo.EntryUser = User.UserName;
                    eventInfo.EntryDate = DateTime.Now;
                    List<EventDetail> EventDetails = new List<EventDetail>();
                    foreach (var item in viewModel.EventDetails)
                    {
                        EventDetail eventDetail = new EventDetail();
                        eventDetail.FloorId = item.FloorId.Value;
                        eventDetail.Amount = item.Amount;
                        eventDetail.OrganizationId = User.OrgId;
                        eventDetail.EntryUser = User.UserName;
                        eventDetail.EntryDate = DateTime.Now;
                        eventDetail.OrganizationId = User.OrgId;
                        EventDetails.Add(eventDetail);
                    }
                    eventInfo.EventDetails = EventDetails;
                    db.tblEventInfo.Add(eventInfo);
                }
                else
                {
                    // Edit Part //
                    var eventInfoInDb = db.tblEventInfo.FirstOrDefault(e => e.EventInfoId == viewModel.EventInfoId);
                    // Checking Existency of the Event in the monthly shop charge table
                    var IsExist = db.tblMonthWiseShopCharges.FirstOrDefault(m => m.EventId == eventInfoInDb.EventInfoId) == null ? false : true;
                    if (IsExist == false)
                    {
                        eventInfoInDb.EventName = viewModel.EventName;
                        eventInfoInDb.Amount = viewModel.Amount;
                        eventInfoInDb.AmountBN = Utility.ConvertEnNumToBnNum(viewModel.Amount.ToString());
                        eventInfoInDb.FundInfoId = viewModel.FundInfoId;
                        eventInfoInDb.FundDetailId = viewModel.FundDetailId;
                        eventInfoInDb.UpdateBy = User.UserName;
                        eventInfoInDb.UpdateDate = DateTime.Now;
                        eventInfoInDb.Remarks = viewModel.Remarks;

                        List<long> floorIds = new List<long>();
                        foreach (var item in viewModel.EventDetails)
                        {
                            long floorId = item.FloorId.Value;
                            floorIds.Add(floorId);
                        }

                        var eventDetails = db.tblEventDetail.Where(ed => ed.EventInfoId == viewModel.EventInfoId).AsEnumerable();
                        var floorsInEd = eventDetails.Where(ed => !floorIds.Contains(ed.FloorId));
                        db.tblEventDetail.RemoveRange(floorsInEd);

                        foreach (var item in viewModel.EventDetails)
                        {
                            var floorIdDB = eventDetails.Where(ed => ed.FloorId == item.FloorId).FirstOrDefault();
                            if (floorIdDB == null)
                            {
                                EventDetail eventDetail = new EventDetail();
                                eventDetail.EventInfoId = viewModel.EventInfoId;
                                eventDetail.Amount = viewModel.Amount;
                                eventDetail.EntryUser = User.UserName;
                                eventDetail.EntryDate = DateTime.Now;
                                eventDetail.OrganizationId = User.OrgId;
                                eventDetail.FloorId = item.FloorId.Value;
                                db.tblEventDetail.Add(eventDetail);
                            }
                            else
                            {
                                floorIdDB.Amount = viewModel.Amount;
                                floorIdDB.UpdateBy = User.UserName;
                                floorIdDB.UpdateDate = DateTime.Now;
                            }
                        }
                    }
                }
                isSuccess = db.SaveChanges() > 0;
            }
            return Json(isSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetEventInfoById(long eventInfoId)
        {
            var eventData = db.tblEventInfo.Include("EventDetails").Where(e => e.EventInfoId == eventInfoId && e.OrganizationId == User.OrgId).AsEnumerable();

            VmEventInfo vmEventInfo = eventData.Select(e => new VmEventInfo { EventInfoId = e.EventInfoId, EventName = e.EventName, Amount = e.Amount, FundInfoId = e.FundInfoId, FundDetailId = e.FundDetailId, EventMonth = (new DateTime(e.Year, e.Month, DateTime.Today.Day)).ToString("dd-MMM-yyyy") }).FirstOrDefault();

            List<VmEventList> vmEventList = new List<VmEventList>();
            foreach (var item in eventData.FirstOrDefault().EventDetails)
            {
                VmEventList vmEvent = new VmEventList();
                vmEvent.FloorId = item.FloorId;
                vmEvent.FloorName = db.tblFloors.FirstOrDefault(f => f.FloorId == item.FloorId).FloorNo;
                vmEventList.Add(vmEvent);
            }
            vmEventInfo.EventDetails = vmEventList;
            return Json(vmEventInfo);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsEventExistinMonthlyCharge(long eventInfoId)
        {
            return Json("");
        }

        #endregion

        private void GetReportFileByOneDataSource(string path, object reportData, string dataSourceName, string rptType, out byte[] fileBytes, out string fileMimeType)
        {
            fileBytes = null;
            fileMimeType = null;
            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath(path);
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;
                ReportDataSource dataSource1 = new ReportDataSource(dataSourceName, reportData);
                localReport.DataSources.Add(dataSource1);
                string reportType = rptType;
                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;

                var renderedBytes = localReport.Render(
                    reportType,
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                fileBytes = renderedBytes;
                fileMimeType = mimeType;
            }
        }

        private void GetReportFileByTwoDataSource(string path, object reportData1, string dataSourceName1, object reportData2, string dataSourceName2, string rptType, out byte[] fileBytes, out string fileMimeType)
        {
            fileBytes = null;
            fileMimeType = null;
            LocalReport localReport = new LocalReport();
            string reportPath = Server.MapPath(path);
            if (System.IO.File.Exists(reportPath))
            {
                localReport.ReportPath = reportPath;

                ReportDataSource dataSource1 = new ReportDataSource(dataSourceName1, reportData1);
                localReport.DataSources.Add(dataSource1);

                ReportDataSource dataSource2 = new ReportDataSource(dataSourceName2, reportData2);
                localReport.DataSources.Add(dataSource2);

                string reportType = rptType;
                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;

                var renderedBytes = localReport.Render(
                    reportType,
                    "",
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                fileBytes = renderedBytes;
                fileMimeType = mimeType;
            }
        }
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