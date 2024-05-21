using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesafioBackEnd.Infrastructure.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalRentValueToRent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalRentValue",
                table: "Rents",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalRentValue",
                table: "Rents");
        }
    }
}
