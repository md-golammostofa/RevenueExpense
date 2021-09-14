using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Caching;
using RevenueAndExpense.DAL.DataBaseContext;
using RevenueAndExpense.BLL.Utility;
using RevenueAndExpense.BO.Models;
using RevenueAndExpense.BO.ViewModels;

namespace RevenueAndExpense.DAL.Security
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public string UserConfigKey { get; set; }
        public string RoleConfigKey { get; set; }

        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        private ApplicationDbContext db = new ApplicationDbContext();

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (HttpContext.Current.Session["UserName"] != null)
            {
                if (filterContext.HttpContext.Request.IsAuthenticated)
                {
                    CustomPrincipalSerializeModel User = (CustomPrincipalSerializeModel)HttpContext.Current.Session["UserDetail"];
                    string action = filterContext.RouteData.Values["action"].ToString();
                    string controller = filterContext.RouteData.Values["controller"].ToString();
                    var navbar = (List<VmUserSubmenuList>)HttpContext.Current.Session["UserSubmenu"];
                    
                    if (controller == "Common")
                    {
                        //
                    }
                    else
                    {
                        var menu = navbar.FirstOrDefault(sub => sub.ControllerName == controller && sub.ActionName == action);
                        if (menu == null)
                        {
                            var submenu =db.tblSubmenus.FirstOrDefault(sub => sub.ControllerName == controller && sub.ActionName == action);
                            if (submenu != null)
                            {
                                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                            }
                        }
                    }
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new
                       RouteValueDictionary(new { controller = "Account", action = "LogIn" }));
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { controller = "Account", action = "LogIn" }));
            }

        }
    }
}