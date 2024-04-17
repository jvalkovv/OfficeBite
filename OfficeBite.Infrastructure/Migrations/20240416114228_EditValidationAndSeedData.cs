﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeBite.Data.Migrations
{
    public partial class EditValidationAndSeedData : Migration
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
                oldComment: "Description of selected menu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MenuOrders",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Description of selected menu",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldComment: "Description of selected menu");
        }
    }
}
