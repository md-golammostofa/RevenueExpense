namespace RevenueAndExpense.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllEntities_7Mar2020 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblBanks",
                c => new
                    {
                        BankId = c.Long(nullable: false, identity: true),
                        BankName = c.String(maxLength: 150),
                        BranchName = c.String(maxLength: 200),
                        AccountNo = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        EntryUser = c.String(maxLength: 50),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                        OrganizationId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.BankId)
                .ForeignKey("dbo.tblOrganizations", t => t.OrganizationId, cascadeDelete: true)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.tblCashToBankAndBankToCashes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BankToCash = c.Double(),
                        CashToBank = c.Double(),
                        Remark = c.String(maxLength: 100),
                        BankDate = c.String(maxLength: 50),
                        EntryUser = c.String(maxLength: 50),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                        BankId = c.Long(),
                        FundInfoId = c.Long(),
                        OrganizationId = c.Long(nullable: false),
                        ChequeNo = c.String(maxLength: 50),
                        IsOpeningBalance = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblBanks", t => t.BankId)
                .ForeignKey("dbo.tblFundInfo", t => t.FundInfoId)
                .Index(t => t.BankId)
                .Index(t => t.FundInfoId);
            
            CreateTable(
                "dbo.tblFundInfo",
                c => new
                    {
                        FundInfoId = c.Long(nullable: false, identity: true),
                        FundName = c.String(maxLength: 100),
                        FundNameBN = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        EntryUser = c.String(maxLength: 50),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                        OrganizationId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.FundInfoId)
                .ForeignKey("dbo.tblOrganizations", t => t.OrganizationId, cascadeDelete: true)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.tblFundDetail",
                c => new
                    {
                        FundDetailId = c.Long(nullable: false, identity: true),
                        FundDetailName = c.String(maxLength: 200),
                        FundDetailNameBN = c.String(maxLength: 200),
                        AmountEN = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountBN = c.String(maxLength: 30),
                        Type = c.String(maxLength: 50),
                        TypeBN = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        EntryUser = c.String(maxLength: 50),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                        OrganizationId = c.Long(),
                        IsFan = c.Boolean(nullable: false),
                        OpeningBalance = c.Decimal(precision: 18, scale: 2),
                        IsMonthlyChargeable = c.Boolean(nullable: false),
                        FundInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.FundDetailId)
                .ForeignKey("dbo.tblFundInfo", t => t.FundInfoId, cascadeDelete: true)
                .Index(t => t.FundInfoId);
            
            CreateTable(
                "dbo.tblRevenueExpenses",
                c => new
                    {
                        RevExId = c.Long(nullable: false, identity: true),
                        RevExName = c.String(maxLength: 200),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FundInfoId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        BillDate = c.DateTime(),
                        EntryUser = c.String(maxLength: 50),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                        Remark = c.String(maxLength: 150),
                        AmountBN = c.String(maxLength: 30),
                        IsOpeningBalance = c.Boolean(nullable: false),
                        StateStatus = c.String(maxLength: 30),
                        BillNo = c.String(maxLength: 30),
                        PayFromOrPayTo = c.String(maxLength: 150),
                        FundDetailId = c.Long(),
                    })
                .PrimaryKey(t => t.RevExId)
                .ForeignKey("dbo.tblFundDetail", t => t.FundDetailId)
                .Index(t => t.FundDetailId);
            
            CreateTable(
                "dbo.tblOrganizations",
                c => new
                    {
                        OrganizationId = c.Long(nullable: false, identity: true),
                        OrganizationName = c.String(maxLength: 100),
                        OrganizationNameBN = c.String(maxLength: 200),
                        Address = c.String(maxLength: 200),
                        AddressBN = c.String(maxLength: 200),
                        Email = c.String(maxLength: 100),
                        MobileNo = c.String(maxLength: 50),
                        PhoneNo = c.String(maxLength: 50),
                        Fax = c.String(maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        EntryUser = c.String(maxLength: 50),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                        RegistrationBN = c.String(maxLength: 150),
                        LicenceBN = c.String(maxLength: 150),
                        Registration = c.String(maxLength: 150),
                        Licence = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.OrganizationId);
            
            CreateTable(
                "dbo.tblFloors",
                c => new
                    {
                        FloorId = c.Long(nullable: false, identity: true),
                        FloorNo = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 100),
                        EntryUser = c.String(maxLength: 50),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                        OrganizationId = c.Long(),
                    })
                .PrimaryKey(t => t.FloorId)
                .ForeignKey("dbo.tblOrganizations", t => t.OrganizationId)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.tblHoldings",
                c => new
                    {
                        HoldingId = c.Long(nullable: false, identity: true),
                        HoldingName = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 150),
                        EntryUser = c.String(maxLength: 50),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                        FloorId = c.Long(),
                        OrganizationId = c.Long(),
                    })
                .PrimaryKey(t => t.HoldingId)
                .ForeignKey("dbo.tblFloors", t => t.FloorId)
                .ForeignKey("dbo.tblOrganizations", t => t.OrganizationId)
                .Index(t => t.FloorId)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.tblShops",
                c => new
                    {
                        ShopId = c.Long(nullable: false, identity: true),
                        ShopName = c.String(maxLength: 100),
                        ProprietorName = c.String(maxLength: 100),
                        MobileNo = c.String(maxLength: 100),
                        PhoneNo = c.String(maxLength: 100),
                        Email = c.String(maxLength: 100),
                        HomeAddress = c.String(maxLength: 150),
                        RegistrationNo = c.String(maxLength: 100),
                        StateStatus = c.String(maxLength: 50),
                        EntryUser = c.String(maxLength: 50),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                        FloorId = c.Long(),
                        OrganizationId = c.Long(),
                        LandOwner = c.String(maxLength: 100),
                        LandOwnerMobile = c.String(maxLength: 50),
                        LandOwnerAddress = c.String(maxLength: 200),
                        LeaseholderName = c.String(maxLength: 100),
                        LeaseholderAddress = c.String(maxLength: 200),
                        LeaseholderPhone = c.String(maxLength: 50),
                        ImagePath = c.String(),
                        NIDNo = c.String(maxLength: 50),
                        FilePath = c.String(),
                        MeterNo = c.String(maxLength: 200),
                        ShopSize = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ShopId)
                .ForeignKey("dbo.tblFloors", t => t.FloorId)
                .ForeignKey("dbo.tblOrganizations", t => t.OrganizationId)
                .Index(t => t.FloorId)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.tblMonthWiseShopCharges",
                c => new
                    {
                        ChargeId = c.Long(nullable: false, identity: true),
                        ChargeName = c.String(maxLength: 200),
                        PreviousReading = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrentReading = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ConsumUnit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FundInfoId = c.Long(nullable: false),
                        FundDetailId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        Month = c.Short(nullable: false),
                        Year = c.Short(nullable: false),
                        EntryUser = c.String(maxLength: 50),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                        Remark = c.String(maxLength: 30),
                        AmountBN = c.String(maxLength: 30),
                        ShopId = c.Long(nullable: false),
                        StateStatus = c.String(maxLength: 30),
                        BillNo = c.String(maxLength: 30),
                        BillExpireDate = c.String(maxLength: 30),
                        InvoiceNo = c.String(maxLength: 50),
                        IsItEventCharge = c.Boolean(),
                        EventId = c.Long(),
                    })
                .PrimaryKey(t => t.ChargeId)
                .ForeignKey("dbo.tblShops", t => t.ShopId, cascadeDelete: true)
                .Index(t => t.ShopId);
            
            CreateTable(
                "dbo.tblShopCharges",
                c => new
                    {
                        ShopChargeId = c.Long(nullable: false, identity: true),
                        FundInfoId = c.Long(nullable: false),
                        FundDetailId = c.Long(nullable: false),
                        ChargeName = c.String(maxLength: 200),
                        ChargeAmountEN = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ChargeAmountBN = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EntryUser = c.String(maxLength: 50),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                        ShopId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ShopChargeId)
                .ForeignKey("dbo.tblShops", t => t.ShopId, cascadeDelete: true)
                .Index(t => t.ShopId);
            
            CreateTable(
                "dbo.tblUsers",
                c => new
                    {
                        UserId = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        UserName = c.String(maxLength: 50),
                        Address = c.String(maxLength: 150),
                        Email = c.String(maxLength: 100),
                        MobileNo = c.String(maxLength: 50),
                        Password = c.String(),
                        ConfirmPassword = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsRoleActive = c.Boolean(nullable: false),
                        EntryUser = c.String(maxLength: 50),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                        OrganizationId = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.tblOrganizations", t => t.OrganizationId, cascadeDelete: true)
                .ForeignKey("dbo.tblRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.OrganizationId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.tblRoles",
                c => new
                    {
                        RoleId = c.Long(nullable: false, identity: true),
                        RoleName = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        EntryUser = c.String(maxLength: 50),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.tblEventDetail",
                c => new
                    {
                        EventDetailId = c.Long(nullable: false, identity: true),
                        FloorId = c.Long(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EntryUser = c.String(maxLength: 150),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                        OrganizationId = c.Long(nullable: false),
                        EventInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.EventDetailId)
                .ForeignKey("dbo.tblEventInfo", t => t.EventInfoId, cascadeDelete: true)
                .Index(t => t.EventInfoId);
            
            CreateTable(
                "dbo.tblEventInfo",
                c => new
                    {
                        EventInfoId = c.Long(nullable: false, identity: true),
                        EventName = c.String(maxLength: 150),
                        Month = c.Short(nullable: false),
                        Year = c.Short(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountBN = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 150),
                        FundInfoId = c.Long(nullable: false),
                        FundDetailId = c.Long(nullable: false),
                        EntryUser = c.String(maxLength: 150),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                        OrganizationId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.EventInfoId);
            
            CreateTable(
                "dbo.tblInvoiceDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        InvoiceNo = c.String(maxLength: 100),
                        ShopId = c.String(maxLength: 100),
                        Month = c.Short(nullable: false),
                        Year = c.Short(nullable: false),
                        ChargeAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FineAmount = c.Decimal(precision: 18, scale: 2),
                        ConnectionFee = c.Decimal(precision: 18, scale: 2),
                        NetAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ChargeAmountBN = c.String(maxLength: 50),
                        FineAmountBN = c.String(maxLength: 20),
                        ConnectionFeeBN = c.String(maxLength: 20),
                        NetAmountBN = c.String(maxLength: 50),
                        EntryUser = c.String(maxLength: 50),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                        BillExpireDate = c.String(maxLength: 50),
                        OrganizationId = c.Long(nullable: false),
                        InvoiceInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblInvoiceInfo", t => t.InvoiceInfoId, cascadeDelete: true)
                .Index(t => t.InvoiceInfoId);
            
            CreateTable(
                "dbo.tblInvoiceInfo",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        InvoiceNo = c.String(maxLength: 100),
                        FloorId = c.Long(),
                        FloorNo = c.String(maxLength: 100),
                        HoldingId = c.Long(),
                        HoldingNo = c.String(maxLength: 100),
                        ShopId = c.Long(nullable: false),
                        TotalFineAmount = c.Decimal(precision: 18, scale: 2),
                        TotalConnectionFee = c.Decimal(precision: 18, scale: 2),
                        TotalChargeAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalFineAmountBN = c.String(maxLength: 20),
                        TotalConnectionFeeBN = c.String(maxLength: 20),
                        TotalChargeAmountBN = c.String(maxLength: 50),
                        TotalAmountBN = c.String(maxLength: 50),
                        EntryUser = c.String(maxLength: 50),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                        OrganizationId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblOrganizations", t => t.OrganizationId, cascadeDelete: true)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.tblMainmenus",
                c => new
                    {
                        MenuId = c.Long(nullable: false, identity: true),
                        MenuName = c.String(maxLength: 100),
                        IconClass = c.String(maxLength: 100),
                        EntryUser = c.String(maxLength: 50),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.MenuId);
            
            CreateTable(
                "dbo.tblSubmenus",
                c => new
                    {
                        SubmenuId = c.Long(nullable: false, identity: true),
                        SubmenuName = c.String(maxLength: 100),
                        IconClass = c.String(maxLength: 100),
                        IsViewable = c.Boolean(nullable: false),
                        EntryUser = c.String(maxLength: 50),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                        MainmenuId = c.Long(nullable: false),
                        ControllerName = c.String(maxLength: 150),
                        ActionName = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.SubmenuId)
                .ForeignKey("dbo.tblMainmenus", t => t.MainmenuId, cascadeDelete: true)
                .Index(t => t.MainmenuId);
            
            CreateTable(
                "dbo.tblShopHolding",
                c => new
                    {
                        ShopId = c.Long(nullable: false),
                        HoldingId = c.Long(nullable: false),
                        EntryUser = c.String(maxLength: 50),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                        OrganizationId = c.Long(),
                        FloorId = c.Long(),
                    })
                .PrimaryKey(t => new { t.ShopId, t.HoldingId })
                .ForeignKey("dbo.tblHoldings", t => t.HoldingId, cascadeDelete: true)
                .ForeignKey("dbo.tblShops", t => t.ShopId, cascadeDelete: true)
                .Index(t => t.ShopId)
                .Index(t => t.HoldingId);
            
            CreateTable(
                "dbo.tblUserMenus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OrganizationId = c.Long(nullable: false),
                        MainmenuId = c.Long(nullable: false),
                        SubmenuId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        UserName = c.String(maxLength: 50),
                        EntryUser = c.String(maxLength: 50),
                        EntryDate = c.DateTime(),
                        UpdateBy = c.String(maxLength: 50),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblShopHolding", "ShopId", "dbo.tblShops");
            DropForeignKey("dbo.tblShopHolding", "HoldingId", "dbo.tblHoldings");
            DropForeignKey("dbo.tblSubmenus", "MainmenuId", "dbo.tblMainmenus");
            DropForeignKey("dbo.tblInvoiceDetails", "InvoiceInfoId", "dbo.tblInvoiceInfo");
            DropForeignKey("dbo.tblInvoiceInfo", "OrganizationId", "dbo.tblOrganizations");
            DropForeignKey("dbo.tblEventDetail", "EventInfoId", "dbo.tblEventInfo");
            DropForeignKey("dbo.tblBanks", "OrganizationId", "dbo.tblOrganizations");
            DropForeignKey("dbo.tblCashToBankAndBankToCashes", "FundInfoId", "dbo.tblFundInfo");
            DropForeignKey("dbo.tblFundInfo", "OrganizationId", "dbo.tblOrganizations");
            DropForeignKey("dbo.tblUsers", "RoleId", "dbo.tblRoles");
            DropForeignKey("dbo.tblUsers", "OrganizationId", "dbo.tblOrganizations");
            DropForeignKey("dbo.tblShopCharges", "ShopId", "dbo.tblShops");
            DropForeignKey("dbo.tblShops", "OrganizationId", "dbo.tblOrganizations");
            DropForeignKey("dbo.tblMonthWiseShopCharges", "ShopId", "dbo.tblShops");
            DropForeignKey("dbo.tblShops", "FloorId", "dbo.tblFloors");
            DropForeignKey("dbo.tblFloors", "OrganizationId", "dbo.tblOrganizations");
            DropForeignKey("dbo.tblHoldings", "OrganizationId", "dbo.tblOrganizations");
            DropForeignKey("dbo.tblHoldings", "FloorId", "dbo.tblFloors");
            DropForeignKey("dbo.tblRevenueExpenses", "FundDetailId", "dbo.tblFundDetail");
            DropForeignKey("dbo.tblFundDetail", "FundInfoId", "dbo.tblFundInfo");
            DropForeignKey("dbo.tblCashToBankAndBankToCashes", "BankId", "dbo.tblBanks");
            DropIndex("dbo.tblShopHolding", new[] { "HoldingId" });
            DropIndex("dbo.tblShopHolding", new[] { "ShopId" });
            DropIndex("dbo.tblSubmenus", new[] { "MainmenuId" });
            DropIndex("dbo.tblInvoiceInfo", new[] { "OrganizationId" });
            DropIndex("dbo.tblInvoiceDetails", new[] { "InvoiceInfoId" });
            DropIndex("dbo.tblEventDetail", new[] { "EventInfoId" });
            DropIndex("dbo.tblUsers", new[] { "RoleId" });
            DropIndex("dbo.tblUsers", new[] { "OrganizationId" });
            DropIndex("dbo.tblShopCharges", new[] { "ShopId" });
            DropIndex("dbo.tblMonthWiseShopCharges", new[] { "ShopId" });
            DropIndex("dbo.tblShops", new[] { "OrganizationId" });
            DropIndex("dbo.tblShops", new[] { "FloorId" });
            DropIndex("dbo.tblHoldings", new[] { "OrganizationId" });
            DropIndex("dbo.tblHoldings", new[] { "FloorId" });
            DropIndex("dbo.tblFloors", new[] { "OrganizationId" });
            DropIndex("dbo.tblRevenueExpenses", new[] { "FundDetailId" });
            DropIndex("dbo.tblFundDetail", new[] { "FundInfoId" });
            DropIndex("dbo.tblFundInfo", new[] { "OrganizationId" });
            DropIndex("dbo.tblCashToBankAndBankToCashes", new[] { "FundInfoId" });
            DropIndex("dbo.tblCashToBankAndBankToCashes", new[] { "BankId" });
            DropIndex("dbo.tblBanks", new[] { "OrganizationId" });
            DropTable("dbo.tblUserMenus");
            DropTable("dbo.tblShopHolding");
            DropTable("dbo.tblSubmenus");
            DropTable("dbo.tblMainmenus");
            DropTable("dbo.tblInvoiceInfo");
            DropTable("dbo.tblInvoiceDetails");
            DropTable("dbo.tblEventInfo");
            DropTable("dbo.tblEventDetail");
            DropTable("dbo.tblRoles");
            DropTable("dbo.tblUsers");
            DropTable("dbo.tblShopCharges");
            DropTable("dbo.tblMonthWiseShopCharges");
            DropTable("dbo.tblShops");
            DropTable("dbo.tblHoldings");
            DropTable("dbo.tblFloors");
            DropTable("dbo.tblOrganizations");
            DropTable("dbo.tblRevenueExpenses");
            DropTable("dbo.tblFundDetail");
            DropTable("dbo.tblFundInfo");
            DropTable("dbo.tblCashToBankAndBankToCashes");
            DropTable("dbo.tblBanks");
        }
    }
}
