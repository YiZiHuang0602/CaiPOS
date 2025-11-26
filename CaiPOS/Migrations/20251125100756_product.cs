using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaiPOS.Migrations
{
    /// <inheritdoc />
    public partial class product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "ShoppingCarItems");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "ShoppingCar",
                newName: "UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "ShoppingCar",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ShoppingCar");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ShoppingCar",
                newName: "UserID");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "ShoppingCarItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
