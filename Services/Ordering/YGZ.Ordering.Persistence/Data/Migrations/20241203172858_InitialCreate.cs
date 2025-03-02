﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YGZ.Ordering.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Storage = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderCode = table.Column<string>(type: "text", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    PaymentType = table.Column<int>(type: "integer", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BillingAddress_AddressLine = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BillingAddress_ContactEmail = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BillingAddress_ContactName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BillingAddress_ContactPhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BillingAddress_Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BillingAddress_District = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BillingAddress_Province = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ShippingAddress_AddressLine = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ShippingAddress_ContactEmail = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ShippingAddress_ContactName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ShippingAddress_ContactPhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ShippingAddress_Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ShippingAddress_District = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ShippingAddress_Province = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<string>(type: "text", nullable: false),
                    ProductModel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProductColor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProductStorage = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    ProductImageUrl = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    SubTotal = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderLines_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_OrderId",
                table: "OrderLines",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_ProductId",
                table: "OrderLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderLines");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
