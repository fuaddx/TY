using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pustok2.Migrations
{
    public partial class SettingUptade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "Email", "Number" },
                values: new object[] { "Azerbaijan Baku, HH2 BacHa, New York, USA", "Fuad@hastech.com", "+1994 5077234 5678" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "Email", "Number" },
                values: new object[] { "Example Street 98, HH2 BacHa, New York, USA", "suport@hastech.com", "+18088 234 5678" });
        }
    }
}
