using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaiPOS.Migrations
{
    /// <inheritdoc />
    public partial class pName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "ShoppingCar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "ShoppingCar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
