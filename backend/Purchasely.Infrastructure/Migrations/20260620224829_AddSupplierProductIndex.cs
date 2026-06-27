using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purchasely.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplierProductIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_supplier_products_SupplierId",
                table: "supplier_products");

            migrationBuilder.CreateIndex(
                name: "IX_supplier_products_SupplierId_ProductId",
                table: "supplier_products",
                columns: new[] { "SupplierId", "ProductId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_supplier_products_SupplierId_ProductId",
                table: "supplier_products");

            migrationBuilder.CreateIndex(
                name: "IX_supplier_products_SupplierId",
                table: "supplier_products",
                column: "SupplierId");
        }
    }
}
