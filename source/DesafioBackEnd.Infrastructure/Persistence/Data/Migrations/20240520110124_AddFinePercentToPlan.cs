using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesafioBackEnd.Infrastructure.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFinePercentToPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "FinePercent",
                table: "Plans",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinePercent",
                table: "Plans");
        }
    }
}
