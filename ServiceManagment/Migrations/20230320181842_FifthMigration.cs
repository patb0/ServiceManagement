using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManagment.Migrations
{
    /// <inheritdoc />
    public partial class FifthMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WorkerId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContactId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_WorkerId",
                table: "Orders",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ContactId",
                table: "AspNetUsers",
                column: "ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Contacts_ContactId",
                table: "AspNetUsers",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_WorkerId",
                table: "Orders",
                column: "WorkerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Contacts_ContactId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_WorkerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_WorkerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ContactId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");
        }
    }
}
