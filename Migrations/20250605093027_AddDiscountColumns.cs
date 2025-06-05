using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartnerControlAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscountColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FinalAmount",
                table: "Transactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TotalDiscount",
                table: "Transactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalAmount",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TotalDiscount",
                table: "Transactions");
        }
    }
}
