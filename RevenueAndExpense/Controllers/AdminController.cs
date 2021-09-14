using RevenueAndExpense.BLL.Filters;
using RevenueAndExpense.BLL.Utility;
using RevenueAndExpense.BO.Models;
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
    public class AdminController : BaseController
    {
        // GET: Admin
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.DashBoardTodayFundRevenue = db.Database.SqlQuery<VmDashBoardTodayFundRevenue>(string.Format(@"Exec spDashBoardTodayFundRevenue {0}", User.OrgId)).ToList();

            ViewBag.DashBoardThisMonthFundRevenue = db.Database.SqlQuery<VmDashBoardTodayFundRevenue>(string.Format(@"Exec spDashBoardThisMonthFundRevenue {0}", User.OrgId)).ToList();

            ViewBag.DashboardMonthlyBillDueCount = db.Database.SqlQuery<VmDashBoardMonthlyBillDueCount>(string.Format(@"Exec spDashboardMonthlyBillDueCount {0}", User.OrgId)).ToList();
            
            ViewBag.DashBoardFundBalance= db.Database.SqlQuery<VmDashBoardFundBalance>(string.Format(@"Exec spFundBalance {0}", User.OrgId)).ToList();

            ViewBag.DashBoardBankBalance = db.Database.SqlQuery<VmDashBoardBankBalance>(string.Format(@"Exec spBankBalance {0}", User.OrgId)).ToList();

            return View();
        }

        [HttpGet]
        public ActionResult GetOrganizationList()
        {
            List<VmOrganization> data = new List<VmOrganization>();
            try
            {
                data = db.tblOrganizations.Select(o => new VmOrganization()
                {
                    OrganizationId = o.OrganizationId,
                    OrganizationName = o.OrganizationName,
                    Address = o.Address,
                    Email = o.Email,
                    MobileNo = o.MobileNo,
                    PhoneNo = o.PhoneNo,
                    Fax = o.Fax,
                    IsActive = o.IsActive,
                    EntryUser = o.EntryUser,
                    EntryDate = o.EntryDate,
                    UpdateBy = o.UpdateBy,
                    UpdateDate = o.UpdateDate
                }).ToList();
            }
            catch (Exception ex)
            {

            }
            return View(data);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveOrganization(VmOrganization model)
        {
            bool isSuccess = false;
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.OrganizationId <= 0)
                    {
                        Organization organization = new Organization
                        {
                            OrganizationId = model.OrganizationId,
                            OrganizationName = model.OrganizationName,
                            Address = model.Address,
                            Email = model.Email,
                            MobileNo = model.MobileNo,
                            PhoneNo = model.PhoneNo,
                            Fax = model.Fax,
                            IsActive = model.IsActive,
                            EntryUser = User.UserName,
                            EntryDate = DateTime.Now,
                            Licence = model.Licence,
                            LicenceBN = model.LicenceBN,
                            OrganizationNameBN = model.OrganizationNameBN,
                            Registration = model.Registration,
                            RegistrationBN = model.RegistrationBN,
                            AddressBN = model.AddressBN
                        };
                        db.tblOrganizations.Add(organization);
                        isSuccess = true;
                    }
                    else
                    {
                        var orgInDb = db.tblOrganizations.FirstOrDefault(o => o.OrganizationId == model.OrganizationId);
                        if (orgInDb != null)
                        {
                            orgInDb.Email = model.Email;
                            orgInDb.PhoneNo = model.PhoneNo;
                            orgInDb.MobileNo = model.MobileNo;
                            orgInDb.Fax = model.Fax;
                            orgInDb.IsActive = model.IsActive;
                            orgInDb.UpdateBy = User.UserName;
                            orgInDb.UpdateDate = DateTime.Now;
                            orgInDb.Address = model.Address;
                            orgInDb.Licence = model.Licence;
                            orgInDb.LicenceBN = model.LicenceBN;
                            orgInDb.OrganizationNameBN = model.OrganizationNameBN;
                            orgInDb.Registration = model.Registration;
                            orgInDb.RegistrationBN = model.RegistrationBN;
                            orgInDb.AddressBN = model.AddressBN;
                            isSuccess = true;
                        }
                        else
                        {
                            isSuccess = false;
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

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetOrganizationById(long id)
        {
            VmOrganization organization = new VmOrganization();
            try
            {
                organization = db.tblOrganizations.Where(org => org.OrganizationId == id).Select(org => new VmOrganization { OrganizationId = org.OrganizationId, OrganizationName = org.OrganizationName, Address = org.Address, MobileNo = org.MobileNo, PhoneNo = org.PhoneNo, Email = org.Email, Fax = org.Fax, IsActive = org.IsActive, EntryUser = org.EntryUser, Registration = org.Registration, RegistrationBN = org.RegistrationBN, Licence = org.Licence, LicenceBN = org.LicenceBN, OrganizationNameBN = org.OrganizationNameBN, AddressBN = org.AddressBN }).FirstOrDefault();
            }
            catch (Exception ex)
            {
            }
            return Json(organization);
        }

        [HttpGet]
        public ActionResult GetAllUser()
        {
            var data = db.tblUsers.Include("tblOrganizations").Include("tblRoles").Where(u => 1 == 1)
                .Select(u => new VmUser
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    UserName = u.UserName,
                    Address = u.Address,
                    Email = u.Email,
                    MobileNo = u.MobileNo,
                    Password = u.Password,
                    ConfirmPassword = u.ConfirmPassword,
                    OrganizationId = u.OrganizationId,
                    OrganizationName = u.Organization.OrganizationName,
                    IsActive = u.IsActive,
                    IsRoleActive = u.IsRoleActive,
                    RoleName = u.Role.RoleName,
                    EntryUser = u.EntryUser,
                    EntryDate = u.EntryDate,
                    UpdateBy = u.UpdateBy,
                    UpdateDate = u.UpdateDate
                }).ToList();
            foreach (var item in data)
            {
                item.Password = Utility.Decrypt(item.Password);
                item.ConfirmPassword = Utility.Decrypt(item.ConfirmPassword);
            }
            return View(data);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveUser(VmUser model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.UserId == 0)
                    {
                        RevenueAndExpense.BO.Models.User user = new User
                        {
                            Name = model.Name,
                            Address = model.Address,
                            MobileNo = model.MobileNo,
                            Email = model.Email,
                            OrganizationId = model.OrganizationId,
                            RoleId = model.RoleId,
                            UserName = model.UserName,
                            Password = Utility.Encrypt(model.Password),
                            ConfirmPassword = Utility.Encrypt(model.ConfirmPassword),
                            IsActive = model.IsActive,
                            IsRoleActive = model.IsRoleActive,
                            EntryUser = User.UserName,
                            EntryDate = DateTime.Now
                        };
                        db.tblUsers.Add(user);
                    }
                    else
                    {
                        var user = db.tblUsers.FirstOrDefault(u => u.UserId == model.UserId);
                        if (user != null)
                        {
                            user.UserName = model.UserName;
                            user.Address = model.Address;
                            user.MobileNo = model.MobileNo;
                            user.Email = model.Email;
                            user.OrganizationId = model.OrganizationId;
                            user.RoleId = model.RoleId;
                            user.Password = Utility.Encrypt(model.Password);
                            user.ConfirmPassword = Utility.Encrypt(model.ConfirmPassword);
                            user.IsActive = model.IsActive;
                            user.IsRoleActive = model.IsRoleActive;
                            user.UpdateBy = User.UserName;
                            user.UpdateDate = DateTime.Now;
                        }
                    }
                    IsSuccess = db.SaveChanges() > 0;
                }
                catch (Exception ex)
                {
                    IsSuccess = false;
                }
            }
            return Json(IsSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetUserById(long Id)
        {
            var data = db.tblUsers.Include("tblOrganizations").Include("tblRoles").Where(u => u.UserId == Id)
                .Select(u => new VmUser
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    UserName = u.UserName,
                    Address = u.Address,
                    Email = u.Email,
                    MobileNo = u.MobileNo,
                    Password = u.Password,
                    ConfirmPassword = u.ConfirmPassword,
                    OrganizationId = u.OrganizationId,
                    OrganizationName = u.Organization.OrganizationName,
                    IsActive = u.IsActive,
                    IsRoleActive = u.IsRoleActive,
                    RoleName = u.Role.RoleName,
                    RoleId = u.Role.RoleId
                }).FirstOrDefault();

            if (data != null)
            {
                data.Password = Utility.Decrypt(data.Password);
                data.ConfirmPassword = Utility.Decrypt(data.ConfirmPassword);
            }
            return Json(data);
        }

        [HttpGet]
        public ActionResult GetMainmenu()
        {
            var data = db.tblMainmenus.ToList();
            return View(data);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveMainmenu(VmMainmenu model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                if (model.MenuId <= 0)
                {
                    Mainmenu mainmenu = new Mainmenu
                    {
                        MenuName = model.MenuName,
                        IconClass = model.IconClass,
                        EntryUser = User.UserName,
                        EntryDate = DateTime.Now
                    };
                    db.tblMainmenus.Add(mainmenu);
                }
                else
                {
                    var data = db.tblMainmenus.FirstOrDefault(m => m.MenuId == model.MenuId);
                    if (data != null)
                    {
                        data.MenuName = model.MenuName;
                        data.IconClass = model.IconClass;
                        data.UpdateBy = User.UserName;
                        data.UpdateDate = DateTime.Now;
                    }
                }
                IsSuccess = db.SaveChanges() > 0;
            }
            return Json(IsSuccess);
        }

        [HttpGet]
        public ActionResult GetSubmenu()
        {
            ViewBag.ddlMainMenu = db.tblMainmenus.Select(s => new SelectListItem
            {
                Text = s.MenuName,
                Value = s.MenuId.ToString()
            }).ToList();

            var data = db.tblSubmenus.Select(s => new VmSubmenu
            {
                MenuId = s.MainmenuId,
                MenuName = s.Mainmenu.MenuName,
                SubmenuId = s.SubmenuId,
                SubmenuName = s.SubmenuName,
                EntryUser = User.UserName,
                EntryDate = s.EntryDate,
                IsViewable = s.IsViewable,
                ActionName = s.ActionName,
                ControllerName = s.ControllerName
            }).ToList();
            return View(data);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveSubmenu(VmSubmenu model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                if (model.SubmenuId <= 0)
                {
                    Submenu submenu = new Submenu
                    {
                        SubmenuName = model.SubmenuName,
                        MainmenuId = model.MenuId,
                        ControllerName = model.ControllerName,
                        ActionName = model.ActionName,
                        IsViewable = model.IsViewable,
                        EntryUser = User.UserName,
                        EntryDate = DateTime.Now
                    };
                    db.tblSubmenus.Add(submenu);
                }
                else
                {
                    var data = db.tblSubmenus.FirstOrDefault(s => s.SubmenuId == model.SubmenuId);
                    if (data != null)
                    {
                        data.SubmenuName = model.SubmenuName;
                        data.MainmenuId = model.MenuId;
                        data.ControllerName = model.ControllerName;
                        data.ActionName = model.ActionName;
                        data.UpdateBy = User.UserName;
                        data.UpdateDate = DateTime.Now;
                    }
                }
                IsSuccess = db.SaveChanges() > 0;
            }
            return Json(IsSuccess);
        }

        [HttpGet]
        public ActionResult SetUserAuthorization()
        {
            var userList = db.tblUsers.Where(u => (u.OrganizationId != 1 && u.OrganizationId == User.OrgId) || (1 == 1)).Select(u => new SelectListItem
            {
                Text = u.UserName,
                Value = u.UserId.ToString()
            }).ToList();
            ViewBag.UserList = userList;

            var data = db.tblMainmenus.Include("Submenus").Where(m => m.Submenus.Count > 0).ToList();
            return View(data);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetUserAuth(long userId)
        {
            var data = db.tblUserMenus.Where(u => u.UserId == userId).ToList();
            return Json(data);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveUserAuthorization(UserAuth model)
        {
            bool IsSuccess = false;
            try
            {
                var userdata = db.tblUsers.FirstOrDefault(u => u.UserId == model.UserId);
                if (userdata != null)
                {
                    string submenuId = string.Empty;
                    foreach (var item in model.MenuData)
                    {
                        submenuId += item.SubmenuId + ",";
                    }

                    submenuId = submenuId.Substring(0, submenuId.Length - 1);
                    var data = db.Database.SqlQuery<int>(string.Format(@"Delete From tblUserMenus where SubmenuId NOT IN ({0}) and UserId ={1} SELECT @@ROWCOUNT", submenuId, model.UserId)).Single();

                    foreach (var item in model.MenuData)
                    {
                        var checkSubmenu = db.tblUserMenus.FirstOrDefault(um => um.SubmenuId == item.SubmenuId && um.MainmenuId == item.MainmenuId && um.UserId == model.UserId);

                        if (checkSubmenu == null)
                        {
                            db.tblUserMenus.Add(new UserMenu
                            {
                                MainmenuId = item.MainmenuId,
                                SubmenuId = item.SubmenuId,
                                UserId = model.UserId,
                                EntryUser = User.UserName,
                                EntryDate = DateTime.Now,
                                UserName = userdata.UserName,
                                OrganizationId = userdata.OrganizationId
                            });
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