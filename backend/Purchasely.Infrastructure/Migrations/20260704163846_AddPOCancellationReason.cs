using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purchasely.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPOCancellationReason : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CancellationReason",
                table: "purchase_orders",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "purchase_orders");
        }
    }
}
