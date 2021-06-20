using Microsoft.EntityFrameworkCore.Migrations;

namespace GeneralStockMarket.WebAPI.Migrations
{
    public partial class LimitUnitPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "UnitPrice",
                table: "LimitOptionRequests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "LimitOptionRequests");
        }
    }
}
