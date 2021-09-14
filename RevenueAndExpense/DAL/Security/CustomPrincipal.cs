using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace RevenueAndExpense.DAL.Security
{
    public class CustomPrincipal: IPrincipal
    {
        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            return roles.Any(r => role.Contains(r));
        }

        public CustomPrincipal(string UserName)
        {
            this.Identity = new GenericIdentity(UserName);
        }
        public long UserId { get; set; }
        public string FirstName { get; set; }

        public string UserName { get; set; }
        public string LastName { get; set; }
        public DateTime LogInTime { get; set; }
        public string MacID { get; set; }
        public long OrgId { get; set; }
        public string  OrgName { get; set; }
        public string[] roles { get; set; }
        public long? RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsRoleActive { get; set; }
        // For Images //
        public string OrgLogo { get; set; }
        public string HeaderLogo { get; set; }
        public string[] LogoPaths { get; set; }
        // Org
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string MobileNo { get; set; }
        public string Fax { get; set; }
    }
}