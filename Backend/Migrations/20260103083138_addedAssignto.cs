using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class addedAssignto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddColumn<string>(
                name: "AssignedTo",
                table: "Grievances",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "Grievances",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "Grievances");

            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "Grievances");

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Water related services", "Water Supply" },
                    { 2, "Electricity related services", "Electricity" },
                    { 3, "Municipal services", "Municipal Services" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DepartmentId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "No Water Supply" },
                    { 2, 1, "Water Leakage" },
                    { 3, 2, "Power Cut" },
                    { 4, 2, "Street Light Not Working" },
                    { 5, 3, "Garbage Not Collected" }
                });
        }
    }
}
