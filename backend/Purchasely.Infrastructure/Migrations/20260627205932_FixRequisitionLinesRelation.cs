using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purchasely.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRequisitionLinesRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_requisition_lines_requisitions_RequisitionId",
                table: "requisition_lines");

            migrationBuilder.AddForeignKey(
                name: "FK_requisition_lines_requisitions_RequisitionId",
                table: "requisition_lines",
                column: "RequisitionId",
                principalTable: "requisitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_requisition_lines_requisitions_RequisitionId",
                table: "requisition_lines");

            migrationBuilder.AddForeignKey(
                name: "FK_requisition_lines_requisitions_RequisitionId",
                table: "requisition_lines",
                column: "RequisitionId",
                principalTable: "requisitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
