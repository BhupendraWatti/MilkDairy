using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MIlkDairy.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class changeTypeofProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCart_ShoppingCart_ProductID",
                table: "ShoppingCart");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCart_Product_ProductID",
                table: "ShoppingCart",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCart_Product_ProductID",
                table: "ShoppingCart");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCart_ShoppingCart_ProductID",
                table: "ShoppingCart",
                column: "ProductID",
                principalTable: "ShoppingCart",
                principalColumn: "ID");
        }
    }
}
