using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PCConfigurator.api.Migrations
{
    /// <inheritdoc />
    public partial class AddBuildLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PCBuilds",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "PCBuildLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BuildId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCBuildLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PCBuildLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PCBuildLikes_PCBuilds_BuildId",
                        column: x => x.BuildId,
                        principalTable: "PCBuilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PCBuilds_UserId",
                table: "PCBuilds",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PCBuildLikes_BuildId",
                table: "PCBuildLikes",
                column: "BuildId");

            migrationBuilder.CreateIndex(
                name: "IX_PCBuildLikes_UserId",
                table: "PCBuildLikes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_AspNetUsers_UserId",
                table: "PCBuilds",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_AspNetUsers_UserId",
                table: "PCBuilds");

            migrationBuilder.DropTable(
                name: "PCBuildLikes");

            migrationBuilder.DropIndex(
                name: "IX_PCBuilds_UserId",
                table: "PCBuilds");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PCBuilds");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");
        }
    }
}
