using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesafioBackEnd.Infrastructure.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingStatusToRent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DeliveryDetails_UserId",
                table: "DeliveryDetails");

            migrationBuilder.AddColumn<short>(
                name: "Status",
                table: "Rents",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDetails_UserId",
                table: "DeliveryDetails",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DeliveryDetails_UserId",
                table: "DeliveryDetails");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Rents");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDetails_UserId",
                table: "DeliveryDetails",
                column: "UserId");
        }
    }
}
