using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ViewModels
{
    public class VmNavBar
    {
        public List<MenuItem> MenuItems { get; set; }
    }

    public class MenuItem
    {
        public string MainmenuName { get; set; }
        public List<SubmenuItem> SubmenuItems { get; set; }
    }
    public class SubmenuItem {
        public string SubmenuName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}