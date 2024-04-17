using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeBite.Data.Migrations
{
    public partial class editDbsets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DishName",
                table: "Dishes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "Name of dish",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Name of dish");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Dishes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                comment: "Description of dish",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Description of dish");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DishName",
                table: "Dishes",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Name of dish",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldComment: "Name of dish");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Dishes",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Description of dish",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldComment: "Description of dish");
        }
    }
}
