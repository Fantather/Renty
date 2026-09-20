using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Renty.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Properties_Location",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "District",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Properties");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PropertyTags",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                table: "Properties",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "ShowExactLocation",
                table: "Properties",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlaceId = table.Column<string>(type: "text", nullable: true),
                    FullAddress = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Street = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    District = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Location = table.Column<Point>(type: "geometry(Point, 4326)", nullable: true),
                    CityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_AddressId",
                table: "Properties",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CityId",
                table: "Addresses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Location",
                table: "Addresses",
                column: "Location")
                .Annotation("Npgsql:IndexMethod", "gist");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PlaceId",
                table: "Addresses",
                column: "PlaceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Addresses_AddressId",
                table: "Properties",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Addresses_AddressId",
                table: "Properties");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Properties_AddressId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PropertyTags");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "ShowExactLocation",
                table: "Properties");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Properties",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Properties",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<Point>(
                name: "Location",
                table: "Properties",
                type: "geometry(Point, 4326)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Properties",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_Location",
                table: "Properties",
                column: "Location")
                .Annotation("Npgsql:IndexMethod", "gist");
        }
    }
}
