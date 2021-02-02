using Microsoft.EntityFrameworkCore.Migrations;

namespace Timesheet.DataAccess.MSSQL.Migrations
{
    public partial class addEmployeerSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Bonus", "LastName", "Position", "Salary" },
                values: new object[] { 1, 20000m, "Иванов", 0, 200000m });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Bonus", "LastName", "Position", "Salary" },
                values: new object[] { 2, null, "Сидоров", 2, 1000m });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Bonus", "LastName", "Position", "Salary" },
                values: new object[] { 3, null, "Петров", 1, 120000m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
