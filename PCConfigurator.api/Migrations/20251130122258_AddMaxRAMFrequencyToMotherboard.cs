using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCConfigurator.api.Migrations
{
    /// <inheritdoc />
    public partial class AddMaxRAMFrequencyToMotherboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxRAMFrequency",
                table: "Motherboards",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxRAMFrequency",
                table: "Motherboards");
        }
    }
}
