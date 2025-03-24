using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YGZ.Discount.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductColorName",
                table: "PromotionProducts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProductStorage",
                table: "PromotionProducts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductColorName",
                table: "PromotionProducts");

            migrationBuilder.DropColumn(
                name: "ProductStorage",
                table: "PromotionProducts");
        }
    }
}
