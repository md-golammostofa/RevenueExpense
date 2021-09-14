using RevenueAndExpense.BO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.DAL.DataBaseContext
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext():base("RevenueAndExpense")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organization>().ToTable("tblOrganizations");
            modelBuilder.Entity<Floor>().ToTable("tblFloors");
        }

        public DbSet<Organization> tblOrganizations { get; set; }
        public DbSet<User> tblUsers { get; set; }
        public DbSet<Role> tblRoles { get; set; }
        public DbSet<Floor> tblFloors { get; set; }
        public DbSet<Holding> tblHoldings { get; set; }
        public DbSet<Shop> tblShops { get; set; }
        public DbSet<ShopHolding> tblShopHolding { get; set; }
        public DbSet<Mainmenu> tblMainmenus { get; set; }
        public DbSet<Submenu> tblSubmenus { get; set; }
        public DbSet<FundInfo> tblFundInfo { get; set; }
        public DbSet<FundDetail> tblFundDetail { get; set; }
        public DbSet<ShopCharge> tblShopCharges { get; set; }
        public DbSet<MonthWiseShopCharge> tblMonthWiseShopCharges { get; set; }
        public DbSet<RevenueExpense> tblRevenueExpenses { get; set; }
        public DbSet<UserMenu> tblUserMenus { get; set; }
        public DbSet<Bank> tblBanks { get; set; }
        public DbSet<CashToBankAndBankToCash> tblCashToBankAndBankToCashes { get; set; }
        public DbSet<InvoiceInfo> tblInvoiceInfo { get; set; }
        public DbSet<InvoiceDetail> tblInvoiceDetails { get; set; }

        public DbSet<EventInfo> tblEventInfo { get; set; }
        public DbSet<EventDetail> tblEventDetail { get; set; }
    }
}