using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_P42.Migrations
{
    /// <inheritdoc />
    public partial class OrdersInEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderInPrice",
                table: "ProductVersions",
                type: "int",
                nullable: false,
                defaultValue: 100000);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "OrderInPrice",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderInPrice",
                table: "ProductGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderInPrice",
                table: "ProductVersions");

            migrationBuilder.DropColumn(
                name: "OrderInPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrderInPrice",
                table: "ProductGroups");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
