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
using System.Web.Security;

namespace RevenueAndExpense.Controllers
{
    public class AccountController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Account
        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(VmLogIn model)
        {
            if (ModelState.IsValid)
            {
                model.Password = Utility.Encrypt(model.Password);
                var user = db.tblUsers.SingleOrDefault(s => s.UserName == model.UserName && s.Password == model.Password);
                if (user != null)
                {
                    if (user.IsActive == true)
                    {
                        var org = db.tblOrganizations.SingleOrDefault(s => s.OrganizationId == user.OrganizationId);
                        if (org.IsActive == true)
                        {
                            //----------------//
                            CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                            serializeModel.UserId = user.UserId;
                            serializeModel.FirstName = user.Name;
                            serializeModel.LastName = user.Name;
                            serializeModel.OrgName = org.OrganizationName;
                            serializeModel.OrgId = org.OrganizationId;
                            serializeModel.UserName = user.UserName;
                            serializeModel.LogInTime = DateTime.Now;
                            serializeModel.MacID = string.Empty;
                            serializeModel.RoleId = user.RoleId;
                            serializeModel.RoleName = db.tblRoles.FirstOrDefault(s => s.RoleId == user.RoleId).RoleName;
                            serializeModel.Address = org.Address;
                            serializeModel.Telephone = org.PhoneNo;
                            serializeModel.MobileNo = org.MobileNo;
                            serializeModel.Fax = org.Fax;

                            serializeModel.IsRoleActive = user.IsRoleActive;
                            Session["UserAuth"] = GetUserMenus(user.UserId);

                            //LogoPaths[0] = org.OrgLogoPath == null ? "" : org.OrgLogoPath;
                            //LogoPaths[1] = org.ReportLogoPath == null ? "" : org.ReportLogoPath;

                            //serializeModel.LogoPaths = LogoPaths;

                            //serializeModel.OrgLogo = Utility.GetImage(org.OrgLogoPath);
                            //serializeModel.HeaderLogo = Utility.GetImage(org.ReportLogoPath);
                            string[] roleArray = new string[2];
                            //if (user.RoleId != null)
                            //{
                            //    var roleName = db.tblRoles.SingleOrDefault(r => r.RoleId == user.RoleId).RoleName.ToString();
                            //    dbRoleName = roleName;
                            //    if (!string.IsNullOrEmpty(roleName))
                            //    {
                            //        serializeModel.RoleName = dbRoleName;
                            //    }
                            //    roleArray[0] = dbRoleName;
                            //}
                            serializeModel.roles = roleArray;

                            Session["UserName"] = user.UserName;
                            Session["UserDetail"] = serializeModel;
                            var UserSubmenu = db.Database.SqlQuery<VmUserSubmenuList>(string.Format(@"Select sub.SubmenuId,sub.SubmenuName,sub.ControllerName,sub.ActionName from tblSubmenus sub
Inner Join tblUserMenus us on sub.SubmenuId = us.SubmenuId
Where us.UserId={0}", user.UserId)).ToList();
                            Session["UserSubmenu"] = UserSubmenu;

                            //Session["SubmenuList"] = db.tblSubmenus.ToList();
                            if (user.IsRoleActive)
                            {
                                //if (user.RoleId > 0 && user.RoleId != null)
                                //{
                                //    Session["UserRoleMenu"] = db.tblRoleWiseUserMenu.Where(r => r.RoleId == user.RoleId).ToList();
                                //}
                            }
                            else
                            {
                                //Session["UserCustomMenu"] = db.tblUserAuthorization.Where(u => u.UserId == user.UserId).ToList();
                            }

                            // User DashBoard //
                            if (org.OrganizationName == "PlayOn24")
                            {
                                return RedirectToAction("Index", "Admin");
                            }
                            else
                            {
                                return RedirectToAction("Index", "User");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "প্রতিষ্ঠানটি ইন-অ্যাক্টিভ");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "ইউজারটি ইন-অ্যাক্টিভ");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "ইউজার নাম/পাসওয়ার্ড ভেলিড নয়");
                }
            }
            else
            {
                ModelState.AddModelError("", "ইউজার নাম/পাসওয়ার্ড আবশ্যক");
            }
            return View(model);
        }

        [HttpGet, CustomAuthorize]
        public ActionResult LogOut()
        {
            Session["UserName"] = null;
            Session["UserDetail"] = null;
            //Session["SubmenuList"] = null;
            Session["UserAuth"] = null;
            //if (User.IsRoleActive)
            //{
            //    Session["UserRoleMenu"] = null;
            //}
            //else
            //{
            //    Session["UserCustomMenu"] = null;
            //}
            System.Web.HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("LogIn", "Account", null);
        }

        private VmNavBar GetUserMenus(long userid)
        {
            VmNavBar navBar = new VmNavBar();
            var userAuths = db.tblUserMenus.Where(u => u.UserId == userid).ToList();
            var userMainmenu = userAuths.Select(ua => ua.MainmenuId).Distinct().ToList();
            string mainmenuId = string.Empty;
            foreach (var item in userMainmenu)
            {
                mainmenuId += item + ",";
            }
            mainmenuId = mainmenuId.Substring(0, mainmenuId.Length - 1);
            string query = string.Format(@"Select * From tblMainmenus where MenuId IN ({0})", mainmenuId);
            var mainmenu = db.Database.SqlQuery<Mainmenu>(query).OrderBy(m => m.MenuId).ToList();

            List<MenuItem> menuItems = new List<MenuItem>();

            foreach (var m in mainmenu)

            {
                MenuItem menuItem = new MenuItem();
                List<SubmenuItem> submenuItems = new List<SubmenuItem>();
                var submenu = userAuths.Where(i => i.MainmenuId == m.MenuId).OrderBy(i => i.SubmenuId).ToList();
                foreach (var s in submenu)
                {
                    var sub = db.tblSubmenus.FirstOrDefault(sm => sm.SubmenuId == s.SubmenuId && sm.IsViewable == true);
                    if (sub != null)
                    {
                        SubmenuItem submenuItem = new SubmenuItem()
                        {
                            SubmenuName = sub.SubmenuName,
                            ControllerName = sub.ControllerName,
                            ActionName = sub.ActionName
                        };
                        submenuItems.Add(submenuItem);
                    }
                }

                if (submenuItems.Count > 0)
                {
                    menuItem.MainmenuName = m.MenuName;
                    menuItem.SubmenuItems = submenuItems;
                    menuItems.Add(menuItem);
                }
            }
            navBar.MenuItems = menuItems;
            return navBar;
        }
    }
}