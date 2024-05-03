using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AssetOrders_AssetId",
                table: "AssetOrders",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetContentDistributions_AssetId",
                table: "AssetContentDistributions",
                column: "AssetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssetOrders_AssetId",
                table: "AssetOrders");

            migrationBuilder.DropIndex(
                name: "IX_AssetContentDistributions_AssetId",
                table: "AssetContentDistributions");
        }
    }
}
