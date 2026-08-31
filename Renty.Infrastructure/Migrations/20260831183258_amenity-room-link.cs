using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renty.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class amenityroomlink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AnemitiesId",
                table: "RoomAmenities",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomAmenities_AnemitiesId",
                table: "RoomAmenities",
                column: "AnemitiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAmenities_Amenities_AnemitiesId",
                table: "RoomAmenities",
                column: "AnemitiesId",
                principalTable: "Amenities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomAmenities_Amenities_AnemitiesId",
                table: "RoomAmenities");

            migrationBuilder.DropIndex(
                name: "IX_RoomAmenities_AnemitiesId",
                table: "RoomAmenities");

            migrationBuilder.DropColumn(
                name: "AnemitiesId",
                table: "RoomAmenities");
        }
    }
}
