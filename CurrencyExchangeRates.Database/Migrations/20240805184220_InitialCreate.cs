using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyExchangeRates.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyExchangeRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromCurrencyCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ToCurrencyCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastRefreshed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BidPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AskPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyExchangeRates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeRates_FromCurrencyCode_ToCurrencyCode",
                table: "CurrencyExchangeRates",
                columns: new[] { "FromCurrencyCode", "ToCurrencyCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyExchangeRates");
        }
    }
}
