using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dapper_StoredProcedures.Migrations
{
    /// <inheritdoc />
    public partial class AddTablePaymentSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Payment_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Payment",
                columns: new[] { "OrderId", "Method", "PaymentDate", "Status" },
                values: new object[,]
                {
                    { 1, "CreditCard", new DateTime(2025, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completed" },
                    { 2, "PayPal", new DateTime(2025, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completed" },
                    { 3, "Cash", new DateTime(2025, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending" },
                    { 4, "CreditCard", new DateTime(2025, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending" },
                    { 5, "BankTransfer", new DateTime(2025, 9, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completed" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payment");
        }
    }
}
