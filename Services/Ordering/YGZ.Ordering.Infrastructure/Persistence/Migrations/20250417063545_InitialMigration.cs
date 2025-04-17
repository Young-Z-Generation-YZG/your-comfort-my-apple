using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YGZ.Ordering.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false, defaultValue: "PENDING"),
                    PaymentMethod = table.Column<string>(type: "text", nullable: false),
                    SubTotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: false),
                    ShippingAddressAddressLine = table.Column<string>(type: "text", nullable: false),
                    ShippingAddressContactEmail = table.Column<string>(type: "text", nullable: false),
                    ShippingAddressContactName = table.Column<string>(type: "text", nullable: false),
                    ShippingAddressContactPhoneNumber = table.Column<string>(type: "text", nullable: false),
                    ShippingAddressCountry = table.Column<string>(type: "text", nullable: false),
                    ShippingAddressDistrict = table.Column<string>(type: "text", nullable: false),
                    ShippingAddressProvince = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<string>(type: "text", nullable: false),
                    ProductName = table.Column<string>(type: "text", nullable: false),
                    ProductColorName = table.Column<string>(type: "text", nullable: false),
                    ProductUnitPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    ProductImage = table.Column<string>(type: "text", nullable: false),
                    ProductSlug = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    PromotionIdOrCode = table.Column<string>(type: "text", nullable: true),
                    PromotionEventType = table.Column<string>(type: "text", nullable: true),
                    PromotionTitle = table.Column<string>(type: "text", nullable: true),
                    PromotionDiscountType = table.Column<string>(type: "text", nullable: true),
                    PromotionDiscountValue = table.Column<decimal>(type: "numeric", nullable: true),
                    PromotionDiscountUnitPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    PromotionAppliedProductCount = table.Column<int>(type: "integer", nullable: true),
                    PromotionFinalPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    IsReviewed = table.Column<bool>(type: "boolean", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
