using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PartnerControlAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartnerNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PartnerKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartnerKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PartnerRefNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TotalAmount = table.Column<long>(type: "bigint", nullable: false),
                    TotalDiscount = table.Column<long>(type: "bigint", nullable: false),
                    FinalAmount = table.Column<long>(type: "bigint", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Signature = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartnerItemRef = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<long>(type: "bigint", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemDetails_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Partners",
                columns: new[] { "Id", "IsActive", "PartnerKey", "PartnerNo", "Password" },
                values: new object[,]
                {
                    { 1, true, "FAKEGOOGLE", "FG-00001", "FAKEPASSWORD1234" },
                    { 2, true, "FAKEPEOPLE", "FG-00002", "FAKEPASSWORD4578" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemDetails_TransactionId",
                table: "ItemDetails",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_PartnerKey",
                table: "Partners",
                column: "PartnerKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Partners_PartnerNo",
                table: "Partners",
                column: "PartnerNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PartnerKey_PartnerRefNo",
                table: "Transactions",
                columns: new[] { "PartnerKey", "PartnerRefNo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemDetails");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
