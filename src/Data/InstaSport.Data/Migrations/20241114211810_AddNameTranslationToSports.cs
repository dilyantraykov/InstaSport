using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstaSport.Data.Migrations
{
    public partial class AddNameTranslationToSports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameTranslations",
                table: "Sports",
                type: "nvarchar(max)",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameTranslations",
                table: "Sports");

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1,
                column: "StartingDateTime",
                value: new DateTime(2024, 5, 3, 12, 58, 41, 895, DateTimeKind.Local).AddTicks(5370));

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartingDateTime",
                value: new DateTime(2024, 5, 2, 16, 58, 41, 895, DateTimeKind.Local).AddTicks(5409));

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 3,
                column: "StartingDateTime",
                value: new DateTime(2024, 5, 2, 15, 58, 41, 895, DateTimeKind.Local).AddTicks(5411));
        }
    }
}
