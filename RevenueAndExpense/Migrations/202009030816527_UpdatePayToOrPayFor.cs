namespace RevenueAndExpense.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePayToOrPayFor : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tblRevenueExpenses", "PayFromOrPayTo", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblRevenueExpenses", "PayFromOrPayTo", c => c.String(maxLength: 150));
        }
    }
}
