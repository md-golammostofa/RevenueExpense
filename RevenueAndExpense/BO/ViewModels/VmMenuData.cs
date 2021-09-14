using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ViewModels
{
    public class VmMenuData
    {
        public long MainmenuId { get; set; }
        public long SubmenuId { get; set; }
    }

    public class UserAuth
    {
        public long UserId { get; set; }
        public List<VmMenuData> MenuData { get; set; }
    }
}