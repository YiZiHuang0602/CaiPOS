using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaiPOS.Migrations
{
    /// <inheritdoc />
    public partial class ItemPName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "ShoppingCarItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "ShoppingCarItems");
        }
    }
}
