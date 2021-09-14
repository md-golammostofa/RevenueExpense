using RevenueAndExpense.BO.Models;
using RevenueAndExpense.DAL.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RevenueAndExpense
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AcquireRequestState(Object sender, EventArgs e)
        {
            if (HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session["UserName"] != null && HttpContext.Current.Session["UserDetail"] != null)
                {
                    CustomPrincipalSerializeModel serializeModel = (CustomPrincipalSerializeModel)Session["UserDetail"];
                    CustomPrincipal newUser = new CustomPrincipal(Session["UserName"].ToString());
                    newUser.UserId = serializeModel.UserId;
                    newUser.FirstName = serializeModel.FirstName;
                    newUser.LastName = serializeModel.LastName;
                    newUser.OrgName = serializeModel.OrgName;
                    newUser.UserName = serializeModel.UserName;
                    newUser.OrgId = serializeModel.OrgId;
                    newUser.LogInTime = serializeModel.LogInTime;
                    newUser.MacID = serializeModel.MacID;
                    //newUser.roles = serializeModel.roles;
                    newUser.RoleId = serializeModel.RoleId;
                    newUser.RoleName = serializeModel.RoleName;
                    newUser.IsRoleActive = serializeModel.IsRoleActive;
                    newUser.RoleName= serializeModel.RoleName;
                    //newUser.OrgLogo = serializeModel.OrgLogo;
                    //newUser.HeaderLogo = serializeModel.HeaderLogo;
                    //newUser.LogoPaths = serializeModel.LogoPaths;
                    newUser.Address = serializeModel.Address;
                    newUser.Telephone = serializeModel.Telephone;
                    newUser.MobileNo = serializeModel.MobileNo;
                    newUser.Fax = serializeModel.Fax;
                    HttpContext.Current.User = newUser;
                }
            }
        }

        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }
    }
}
