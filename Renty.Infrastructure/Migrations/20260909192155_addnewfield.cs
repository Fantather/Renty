using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renty.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addnewfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomsCount",
                table: "PropertyDetails",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddCheckConstraint(
                name: "CK_PropertyDetails_RoomsCount_Minimum",
                table: "PropertyDetails",
                sql: "\"RoomsCount\" >= (\"BedsCount\" + \"BedroomsCount\" + \"BathroomsCount\")");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_PropertyDetails_RoomsCount_Minimum",
                table: "PropertyDetails");

            migrationBuilder.DropColumn(
                name: "RoomsCount",
                table: "PropertyDetails");
        }
    }
}
