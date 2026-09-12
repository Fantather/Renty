using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renty.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncApplicationUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameRu",
                table: "Regions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameRu",
                table: "Countries",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameRu",
                table: "Cities",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_NameRu",
                table: "Regions",
                column: "NameRu");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_NameRu",
                table: "Countries",
                column: "NameRu");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_NameRu",
                table: "Cities",
                column: "NameRu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Regions_NameRu",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Countries_NameRu",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Cities_NameRu",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "NameRu",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "NameRu",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "NameRu",
                table: "Cities");
        }
    }
}
