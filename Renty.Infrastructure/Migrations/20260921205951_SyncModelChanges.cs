using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renty.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IconUrl",
                table: "Tags",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IconId",
                table: "PropertiesCategories",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconUrl",
                table: "PropertiesCategories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InstantBook",
                table: "Properties",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "WeekendPricePercent",
                table: "Properties",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconId",
                table: "PropertiesCategories");

            migrationBuilder.DropColumn(
                name: "IconUrl",
                table: "PropertiesCategories");

            migrationBuilder.DropColumn(
                name: "InstantBook",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "WeekendPricePercent",
                table: "Properties");

            migrationBuilder.AlterColumn<Guid>(
                name: "IconUrl",
                table: "Tags",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
