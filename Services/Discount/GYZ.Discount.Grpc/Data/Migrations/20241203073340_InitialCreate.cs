using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GYZ.Discount.Grpc.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DiscountValue = table.Column<double>(type: "double precision", nullable: false),
                    MinPurchaseAmount = table.Column<double>(type: "double precision", nullable: true),
                    MaxDiscountAmount = table.Column<double>(type: "double precision", nullable: true),
                    ValidFrom = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    QuantityRemain = table.Column<int>(type: "integer", nullable: false),
                    UsageLimit = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "Code", "CreatedAt", "DeletedAt", "Description", "DiscountValue", "MaxDiscountAmount", "MinPurchaseAmount", "QuantityRemain", "Status", "Title", "Type", "UpdatedAt", "UsageLimit", "ValidFrom", "ValidTo" },
                values: new object[] { new Guid("d203cbdc-f010-4ec6-9d42-512893ca7006"), "CODE", new DateTime(2024, 12, 3, 7, 33, 39, 449, DateTimeKind.Utc).AddTicks(2328), null, "Summer 2024 description", 0.20000000000000001, null, null, 20, 1, "Summer 2024", 0, new DateTime(2024, 12, 3, 7, 33, 39, 449, DateTimeKind.Utc).AddTicks(2329), 20, new DateTime(2024, 12, 3, 7, 33, 39, 448, DateTimeKind.Utc).AddTicks(9761), new DateTime(2024, 12, 3, 7, 33, 39, 449, DateTimeKind.Utc).AddTicks(543) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");
        }
    }
}
