using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstaSport.Data.Migrations
{
    public partial class AddNameTranslationToLocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameTranslations",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1,
                column: "StartingDateTime",
                value: new DateTime(2024, 11, 15, 19, 41, 23, 198, DateTimeKind.Local).AddTicks(1130));

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartingDateTime",
                value: new DateTime(2024, 11, 14, 23, 41, 23, 198, DateTimeKind.Local).AddTicks(1167));

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 3,
                column: "StartingDateTime",
                value: new DateTime(2024, 11, 14, 22, 41, 23, 198, DateTimeKind.Local).AddTicks(1169));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameTranslations",
                table: "Locations");

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1,
                column: "StartingDateTime",
                value: new DateTime(2024, 11, 15, 19, 18, 10, 651, DateTimeKind.Local).AddTicks(6161));

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartingDateTime",
                value: new DateTime(2024, 11, 14, 23, 18, 10, 651, DateTimeKind.Local).AddTicks(6201));

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 3,
                column: "StartingDateTime",
                value: new DateTime(2024, 11, 14, 22, 18, 10, 651, DateTimeKind.Local).AddTicks(6202));
        }
    }
}
