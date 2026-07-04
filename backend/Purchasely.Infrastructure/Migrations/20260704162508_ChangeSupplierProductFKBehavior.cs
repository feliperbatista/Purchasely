using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purchasely.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSupplierProductFKBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_supplier_products_suppliers_SupplierId",
                table: "supplier_products");

            migrationBuilder.AddForeignKey(
                name: "FK_supplier_products_suppliers_SupplierId",
                table: "supplier_products",
                column: "SupplierId",
                principalTable: "suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_supplier_products_suppliers_SupplierId",
                table: "supplier_products");

            migrationBuilder.AddForeignKey(
                name: "FK_supplier_products_suppliers_SupplierId",
                table: "supplier_products",
                column: "SupplierId",
                principalTable: "suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
