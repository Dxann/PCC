using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCConfigurator.api.Migrations
{
    /// <inheritdoc />
    public partial class PCRAM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RAMId",
                table: "PCBuilds",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PCBuilds_RAMId",
                table: "PCBuilds",
                column: "RAMId");

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_CPUs_RAMId",
                table: "PCBuilds",
                column: "RAMId",
                principalTable: "CPUs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_CPUs_RAMId",
                table: "PCBuilds");

            migrationBuilder.DropIndex(
                name: "IX_PCBuilds_RAMId",
                table: "PCBuilds");

            migrationBuilder.DropColumn(
                name: "RAMId",
                table: "PCBuilds");
        }
    }
}
