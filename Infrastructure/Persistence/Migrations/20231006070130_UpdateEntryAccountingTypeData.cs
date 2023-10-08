using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class UpdateEntryAccountingTypeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add your custom SQL command(s) for the "Up" migration here
            //string sql0 = $"update EntryAccounting set Type = 'MembershipMovement' where Type = 'CloseMemberShipMovement'";
            //migrationBuilder.Sql(sql0);

            //string sql1 = $"update  EntryAccounting set Type = 'PaymentCashPool' where Type = 'ClosePayment' or Type = 'CashPool'";
            //migrationBuilder.Sql(sql1);

            //string sql2 = $"update  EntryAccounting  set Type = 'SalesInvoiceCashPool' where Type = 'CloseChash'";
            //migrationBuilder.Sql(sql2);

            //string sql3 = $"update  EntryAccounting set Type = 'MembershipMovement' where Type = 'Auto'";
            //migrationBuilder.Sql(sql3);  
            
            //string sql4 = $"update EntryMovement  set TableName = 'PaymentCashPool' where TableName ='CashPool'";
            //migrationBuilder.Sql(sql4);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
