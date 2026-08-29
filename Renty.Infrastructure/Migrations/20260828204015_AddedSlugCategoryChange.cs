using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renty.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedSlugCategoryChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconUrl",
                table: "PropertiesCategories");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "PropertiesCategories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "PropertiesCategories",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PropertiesCategories_Slug",
                table: "PropertiesCategories",
                column: "Slug");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PropertiesCategories_Slug",
                table: "PropertiesCategories");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "PropertiesCategories");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "PropertiesCategories");

            migrationBuilder.AddColumn<string>(
                name: "IconUrl",
                table: "PropertiesCategories",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
