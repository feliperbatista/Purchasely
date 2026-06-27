using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purchasely.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixPurchaseOrderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalAmout",
                table: "purchase_orders",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "TaxAmout",
                table: "purchase_orders",
                newName: "TaxAmount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "purchase_orders",
                newName: "TotalAmout");

            migrationBuilder.RenameColumn(
                name: "TaxAmount",
                table: "purchase_orders",
                newName: "TaxAmout");
        }
    }
}
