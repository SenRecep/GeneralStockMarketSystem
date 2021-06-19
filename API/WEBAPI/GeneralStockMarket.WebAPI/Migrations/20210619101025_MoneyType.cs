using Microsoft.EntityFrameworkCore.Migrations;

namespace GeneralStockMarket.WebAPI.Migrations
{
    public partial class MoneyType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "MoneyType",
                table: "DepositRequests",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoneyType",
                table: "DepositRequests");
        }
    }
}
