using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renty.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class floors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Floor",
                table: "PropertyDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FloorsCount",
                table: "PropertyDetails",
                type: "integer",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Floor",
                table: "PropertyDetails");

            migrationBuilder.DropColumn(
                name: "FloorsCount",
                table: "PropertyDetails");
        }
    }
}
