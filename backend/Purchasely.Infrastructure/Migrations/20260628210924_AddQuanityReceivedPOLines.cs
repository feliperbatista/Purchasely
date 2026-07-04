using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purchasely.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddQuanityReceivedPOLines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "QuantityReceived",
                table: "purchase_order_lines",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityReceived",
                table: "purchase_order_lines");
        }
    }
}
