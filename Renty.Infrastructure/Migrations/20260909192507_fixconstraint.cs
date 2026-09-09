using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renty.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixconstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_PropertyDetails_RoomsCount_Minimum",
                table: "PropertyDetails");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PropertyDetails_RoomsCount_Minimum",
                table: "PropertyDetails",
                sql: "\"RoomsCount\" >= (\"BedroomsCount\" + \"BathroomsCount\")");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_PropertyDetails_RoomsCount_Minimum",
                table: "PropertyDetails");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PropertyDetails_RoomsCount_Minimum",
                table: "PropertyDetails",
                sql: "\"RoomsCount\" >= (\"BedsCount\" + \"BedroomsCount\" + \"BathroomsCount\")");
        }
    }
}
