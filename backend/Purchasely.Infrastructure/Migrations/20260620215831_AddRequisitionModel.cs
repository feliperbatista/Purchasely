using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purchasely.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRequisitionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_suppliers_SupplierId",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_SupplierId_SKU",
                table: "products");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "products");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "products",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "requisitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Priority = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Justification = table.Column<string>(type: "text", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RequesterId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_requisitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_requisitions_users_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "requisition_lines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequisitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuantityRequested = table.Column<decimal>(type: "numeric", nullable: false),
                    EstimatedUnitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_requisition_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_requisition_lines_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_requisition_lines_requisitions_RequisitionId",
                        column: x => x.RequisitionId,
                        principalTable: "requisitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_requisition_lines_ProductId",
                table: "requisition_lines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_requisition_lines_RequisitionId",
                table: "requisition_lines",
                column: "RequisitionId");

            migrationBuilder.CreateIndex(
                name: "IX_requisitions_RequesterId",
                table: "requisitions",
                column: "RequesterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "requisition_lines");

            migrationBuilder.DropTable(
                name: "requisitions");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "products");

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "products",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_products_SupplierId_SKU",
                table: "products",
                columns: new[] { "SupplierId", "SKU" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_products_suppliers_SupplierId",
                table: "products",
                column: "SupplierId",
                principalTable: "suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
