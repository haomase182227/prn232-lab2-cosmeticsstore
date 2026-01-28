using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmeticsStore.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteAndCodeFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CosmeticCode",
                table: "CosmeticInformation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CosmeticInformation",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "CosmeticInformation",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CosmeticInformation",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "CategoryCode",
                table: "CosmeticCategory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CosmeticCategory",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "CosmeticCategory",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CosmeticCategory",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CosmeticCode",
                table: "CosmeticInformation");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CosmeticInformation");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CosmeticInformation");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CosmeticInformation");

            migrationBuilder.DropColumn(
                name: "CategoryCode",
                table: "CosmeticCategory");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CosmeticCategory");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CosmeticCategory");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CosmeticCategory");
        }
    }
}
