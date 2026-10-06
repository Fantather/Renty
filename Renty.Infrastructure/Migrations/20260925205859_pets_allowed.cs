using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renty.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class pets_allowed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PetsAllowed",
                table: "PropertyDetails",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PlaceId",
                table: "Cities",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_PlaceId",
                table: "Cities",
                column: "PlaceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cities_PlaceId",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "PetsAllowed",
                table: "PropertyDetails");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "Cities");
        }
    }
}
