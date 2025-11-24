using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaiPOS.Migrations
{
    /// <inheritdoc />
    public partial class FixModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItem",
                table: "CartItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cart",
                table: "Cart");

            migrationBuilder.RenameTable(
                name: "CartItem",
                newName: "ShoppingCarItem");

            migrationBuilder.RenameTable(
                name: "Cart",
                newName: "ShoppingCar");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCarItem",
                table: "ShoppingCarItem",
                column: "CartItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCar",
                table: "ShoppingCar",
                column: "CarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCarItem",
                table: "ShoppingCarItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCar",
                table: "ShoppingCar");

            migrationBuilder.RenameTable(
                name: "ShoppingCarItem",
                newName: "CartItem");

            migrationBuilder.RenameTable(
                name: "ShoppingCar",
                newName: "Cart");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItem",
                table: "CartItem",
                column: "CartItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cart",
                table: "Cart",
                column: "CarId");
        }
    }
}
