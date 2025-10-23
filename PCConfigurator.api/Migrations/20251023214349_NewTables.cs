using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PCConfigurator.api.Migrations
{
    /// <inheritdoc />
    public partial class NewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_CPUs_CPUId",
                table: "PCBuilds");

            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_GPUs_GPUId",
                table: "PCBuilds");

            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_RAMs_RAMId",
                table: "PCBuilds");

            migrationBuilder.DropTable(
                name: "POWERSUPPLYs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_THERMALPASTEs",
                table: "THERMALPASTEs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CASEs",
                table: "CASEs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MOTHERBOBOARDs",
                table: "MOTHERBOBOARDs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PCBuilds");

            migrationBuilder.DropColumn(
                name: "Chipset",
                table: "MOTHERBOBOARDs");

            migrationBuilder.DropColumn(
                name: "FormFactor",
                table: "MOTHERBOBOARDs");

            migrationBuilder.DropColumn(
                name: "Socket",
                table: "MOTHERBOBOARDs");

            migrationBuilder.RenameTable(
                name: "THERMALPASTEs",
                newName: "ThermalPastes");

            migrationBuilder.RenameTable(
                name: "CASEs",
                newName: "Cases");

            migrationBuilder.RenameTable(
                name: "MOTHERBOBOARDs",
                newName: "Motherboards");

            migrationBuilder.RenameColumn(
                name: "MemorySlots",
                table: "Motherboards",
                newName: "Cores");

            migrationBuilder.AlterColumn<int>(
                name: "RAMId",
                table: "PCBuilds",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "GPUId",
                table: "PCBuilds",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "CPUId",
                table: "PCBuilds",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "CaseId",
                table: "PCBuilds",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HDDId",
                table: "PCBuilds",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MotherboardId",
                table: "PCBuilds",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PSUId",
                table: "PCBuilds",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SSDId",
                table: "PCBuilds",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThermalPasteId",
                table: "PCBuilds",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Frequency",
                table: "Motherboards",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThermalPastes",
                table: "ThermalPastes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cases",
                table: "Cases",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Motherboards",
                table: "Motherboards",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PSUs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Wattage = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PSUs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PCBuilds_CaseId",
                table: "PCBuilds",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PCBuilds_HDDId",
                table: "PCBuilds",
                column: "HDDId");

            migrationBuilder.CreateIndex(
                name: "IX_PCBuilds_MotherboardId",
                table: "PCBuilds",
                column: "MotherboardId");

            migrationBuilder.CreateIndex(
                name: "IX_PCBuilds_PSUId",
                table: "PCBuilds",
                column: "PSUId");

            migrationBuilder.CreateIndex(
                name: "IX_PCBuilds_SSDId",
                table: "PCBuilds",
                column: "SSDId");

            migrationBuilder.CreateIndex(
                name: "IX_PCBuilds_ThermalPasteId",
                table: "PCBuilds",
                column: "ThermalPasteId");

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_CPUs_CPUId",
                table: "PCBuilds",
                column: "CPUId",
                principalTable: "CPUs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_Cases_CaseId",
                table: "PCBuilds",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_GPUs_GPUId",
                table: "PCBuilds",
                column: "GPUId",
                principalTable: "GPUs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_HDDs_HDDId",
                table: "PCBuilds",
                column: "HDDId",
                principalTable: "HDDs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_Motherboards_MotherboardId",
                table: "PCBuilds",
                column: "MotherboardId",
                principalTable: "Motherboards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_PSUs_PSUId",
                table: "PCBuilds",
                column: "PSUId",
                principalTable: "PSUs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_RAMs_RAMId",
                table: "PCBuilds",
                column: "RAMId",
                principalTable: "RAMs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_SSDs_SSDId",
                table: "PCBuilds",
                column: "SSDId",
                principalTable: "SSDs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_ThermalPastes_ThermalPasteId",
                table: "PCBuilds",
                column: "ThermalPasteId",
                principalTable: "ThermalPastes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_CPUs_CPUId",
                table: "PCBuilds");

            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_Cases_CaseId",
                table: "PCBuilds");

            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_GPUs_GPUId",
                table: "PCBuilds");

            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_HDDs_HDDId",
                table: "PCBuilds");

            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_Motherboards_MotherboardId",
                table: "PCBuilds");

            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_PSUs_PSUId",
                table: "PCBuilds");

            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_RAMs_RAMId",
                table: "PCBuilds");

            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_SSDs_SSDId",
                table: "PCBuilds");

            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_ThermalPastes_ThermalPasteId",
                table: "PCBuilds");

            migrationBuilder.DropTable(
                name: "PSUs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThermalPastes",
                table: "ThermalPastes");

            migrationBuilder.DropIndex(
                name: "IX_PCBuilds_CaseId",
                table: "PCBuilds");

            migrationBuilder.DropIndex(
                name: "IX_PCBuilds_HDDId",
                table: "PCBuilds");

            migrationBuilder.DropIndex(
                name: "IX_PCBuilds_MotherboardId",
                table: "PCBuilds");

            migrationBuilder.DropIndex(
                name: "IX_PCBuilds_PSUId",
                table: "PCBuilds");

            migrationBuilder.DropIndex(
                name: "IX_PCBuilds_SSDId",
                table: "PCBuilds");

            migrationBuilder.DropIndex(
                name: "IX_PCBuilds_ThermalPasteId",
                table: "PCBuilds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cases",
                table: "Cases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Motherboards",
                table: "Motherboards");

            migrationBuilder.DropColumn(
                name: "CaseId",
                table: "PCBuilds");

            migrationBuilder.DropColumn(
                name: "HDDId",
                table: "PCBuilds");

            migrationBuilder.DropColumn(
                name: "MotherboardId",
                table: "PCBuilds");

            migrationBuilder.DropColumn(
                name: "PSUId",
                table: "PCBuilds");

            migrationBuilder.DropColumn(
                name: "SSDId",
                table: "PCBuilds");

            migrationBuilder.DropColumn(
                name: "ThermalPasteId",
                table: "PCBuilds");

            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "Motherboards");

            migrationBuilder.RenameTable(
                name: "ThermalPastes",
                newName: "THERMALPASTEs");

            migrationBuilder.RenameTable(
                name: "Cases",
                newName: "CASEs");

            migrationBuilder.RenameTable(
                name: "Motherboards",
                newName: "MOTHERBOBOARDs");

            migrationBuilder.RenameColumn(
                name: "Cores",
                table: "MOTHERBOBOARDs",
                newName: "MemorySlots");

            migrationBuilder.AlterColumn<int>(
                name: "RAMId",
                table: "PCBuilds",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GPUId",
                table: "PCBuilds",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CPUId",
                table: "PCBuilds",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PCBuilds",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Chipset",
                table: "MOTHERBOBOARDs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FormFactor",
                table: "MOTHERBOBOARDs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Socket",
                table: "MOTHERBOBOARDs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_THERMALPASTEs",
                table: "THERMALPASTEs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CASEs",
                table: "CASEs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MOTHERBOBOARDs",
                table: "MOTHERBOBOARDs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "POWERSUPPLYs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Wattage = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POWERSUPPLYs", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_CPUs_CPUId",
                table: "PCBuilds",
                column: "CPUId",
                principalTable: "CPUs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_GPUs_GPUId",
                table: "PCBuilds",
                column: "GPUId",
                principalTable: "GPUs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_RAMs_RAMId",
                table: "PCBuilds",
                column: "RAMId",
                principalTable: "RAMs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
