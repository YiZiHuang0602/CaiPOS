using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaiPOS.Migrations
{
    /// <inheritdoc />
    public partial class RenameCartTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserManagement",
                table: "UserManagement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCarItem",
                table: "ShoppingCarItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.RenameTable(
                name: "UserManagement",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "ShoppingCarItem",
                newName: "ShoppingCarItems");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameColumn(
                name: "CartId",
                table: "ShoppingCarItems",
                newName: "CarId");

            migrationBuilder.RenameColumn(
                name: "CartItemId",
                table: "ShoppingCarItems",
                newName: "CarItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCarItems",
                table: "ShoppingCarItems",
                column: "CarItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCarItems",
                table: "ShoppingCarItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "UserManagement");

            migrationBuilder.RenameTable(
                name: "ShoppingCarItems",
                newName: "ShoppingCarItem");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameColumn(
                name: "CarId",
                table: "ShoppingCarItem",
                newName: "CartId");

            migrationBuilder.RenameColumn(
                name: "CarItemId",
                table: "ShoppingCarItem",
                newName: "CartItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserManagement",
                table: "UserManagement",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCarItem",
                table: "ShoppingCarItem",
                column: "CartItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "OrderId");
        }
    }
}
