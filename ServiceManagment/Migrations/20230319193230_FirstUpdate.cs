using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManagment.Migrations
{
    /// <inheritdoc />
    public partial class FirstUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "WorkerId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
