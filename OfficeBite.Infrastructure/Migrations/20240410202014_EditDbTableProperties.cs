using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeBite.Data.Migrations
{
    public partial class EditDbTableProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHistories_UserAgents_UserAgentUserId",
                table: "OrderHistories");

            migrationBuilder.DropIndex(
                name: "IX_OrderHistories_UserAgentUserId",
                table: "OrderHistories");

            migrationBuilder.DropColumn(
                name: "UserAgentUserId",
                table: "OrderHistories");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OrderHistories",
                type: "int",
                nullable: false,
                comment: "Order History identifier",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Order history identifier")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "MenuOrderRequestNumber",
                table: "OrderHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserAgentId",
                table: "OrderHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "User identifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuOrderRequestNumber",
                table: "OrderHistories");

            migrationBuilder.DropColumn(
                name: "UserAgentId",
                table: "OrderHistories");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OrderHistories",
                type: "int",
                nullable: false,
                comment: "Order history identifier",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Order History identifier")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "UserAgentUserId",
                table: "OrderHistories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistories_UserAgentUserId",
                table: "OrderHistories",
                column: "UserAgentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHistories_UserAgents_UserAgentUserId",
                table: "OrderHistories",
                column: "UserAgentUserId",
                principalTable: "UserAgents",
                principalColumn: "UserId");
        }
    }
}
