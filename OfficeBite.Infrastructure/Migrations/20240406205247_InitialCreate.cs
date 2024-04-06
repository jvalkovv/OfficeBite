using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeBite.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DishCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Dish Category Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Name of Category")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Menu Type Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Name of Menu type")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAgents",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "User identifier"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "User First Name"),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "User Last Name"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "User account name")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAgents", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserAgents_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Dish Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DishName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Name of dish"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Price of dish"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Description of dish"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Image of dish"),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false, comment: "Visibility  of dish"),
                    CategoryId = table.Column<int>(type: "int", nullable: false, comment: "Category identifier of dish")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dishes_DishCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "DishCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DishesInMenus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Dishes In Menu Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false, comment: "Visibility  of dishes"),
                    DishId = table.Column<int>(type: "int", nullable: false),
                    RequestMenuNumber = table.Column<int>(type: "int", nullable: false, comment: "Menu identifier"),
                    MenuTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishesInMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DishesInMenus_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishesInMenus_MenuTypes_MenuTypeId",
                        column: x => x.MenuTypeId,
                        principalTable: "MenuTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MenuOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Menu Order id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestMenuNumber = table.Column<int>(type: "int", nullable: false, comment: "Menu order Identifier with other tables"),
                    SelectedMenuDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Menu for date"),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Total price of selected menu"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Description of selected menu"),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false, comment: "Visibility  of menu"),
                    MenuTypeId = table.Column<int>(type: "int", nullable: false, comment: "Menu type identifier"),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuOrders", x => x.Id);
                    table.UniqueConstraint("AK_MenuOrders_RequestMenuNumber", x => x.RequestMenuNumber);
                    table.ForeignKey(
                        name: "FK_MenuOrders_MenuTypes_MenuTypeId",
                        column: x => x.MenuTypeId,
                        principalTable: "MenuTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderPlacedOnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsEaten = table.Column<bool>(type: "bit", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAgentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MenuOrderRequestNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_MenuOrders_MenuOrderRequestNumber",
                        column: x => x.MenuOrderRequestNumber,
                        principalTable: "MenuOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_UserAgents_UserAgentId",
                        column: x => x.UserAgentId,
                        principalTable: "UserAgents",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Order history identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false, comment: "Order id"),
                    UserAgentUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderHistories_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderHistories_UserAgents_UserAgentUserId",
                        column: x => x.UserAgentUserId,
                        principalTable: "UserAgents",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_CategoryId",
                table: "Dishes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DishesInMenus_DishId",
                table: "DishesInMenus",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_DishesInMenus_MenuTypeId",
                table: "DishesInMenus",
                column: "MenuTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DishesInMenus_RequestMenuNumber",
                table: "DishesInMenus",
                column: "RequestMenuNumber");

            migrationBuilder.CreateIndex(
                name: "IX_MenuOrders_MenuTypeId",
                table: "MenuOrders",
                column: "MenuTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuOrders_OrderId",
                table: "MenuOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistories_OrderId",
                table: "OrderHistories",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistories_UserAgentUserId",
                table: "OrderHistories",
                column: "UserAgentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_MenuOrderRequestNumber",
                table: "Orders",
                column: "MenuOrderRequestNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserAgentId",
                table: "Orders",
                column: "UserAgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DishesInMenus_MenuOrders_RequestMenuNumber",
                table: "DishesInMenus",
                column: "RequestMenuNumber",
                principalTable: "MenuOrders",
                principalColumn: "RequestMenuNumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuOrders_Orders_OrderId",
                table: "MenuOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_MenuOrders_MenuOrderRequestNumber",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "DishesInMenus");

            migrationBuilder.DropTable(
                name: "OrderHistories");

            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "DishCategories");

            migrationBuilder.DropTable(
                name: "MenuOrders");

            migrationBuilder.DropTable(
                name: "MenuTypes");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "UserAgents");
        }
    }
}
