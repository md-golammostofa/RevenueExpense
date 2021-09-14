using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ViewModels
{
    public class VmUserSubmenuList
    {
        public long SubmenuId { get; set; }
        public string SubmenuName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}