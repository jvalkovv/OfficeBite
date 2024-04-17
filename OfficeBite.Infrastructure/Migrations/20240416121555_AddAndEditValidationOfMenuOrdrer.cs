using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeBite.Data.Migrations
{
    public partial class AddAndEditValidationOfMenuOrdrer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MenuOrders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                comment: "Description of selected menu",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldComment: "Description of selected menu");

            migrationBuilder.AddColumn<string>(
                name: "MenuName",
                table: "MenuOrders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                comment: "Menu name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuName",
                table: "MenuOrders");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MenuOrders",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: false,
                comment: "Description of selected menu",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldComment: "Description of selected menu");
        }
    }
}
