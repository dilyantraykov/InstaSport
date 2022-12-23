using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstaSport.Data.Migrations
{
    public partial class UpdateRatings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Rating",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1,
                column: "StartingDateTime",
                value: new DateTime(2022, 4, 8, 12, 57, 17, 70, DateTimeKind.Local).AddTicks(1736));

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartingDateTime",
                value: new DateTime(2022, 4, 7, 16, 57, 17, 70, DateTimeKind.Local).AddTicks(1771));

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 3,
                column: "StartingDateTime",
                value: new DateTime(2022, 4, 7, 15, 57, 17, 70, DateTimeKind.Local).AddTicks(1773));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Rating");

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1,
                column: "StartingDateTime",
                value: new DateTime(2022, 1, 3, 13, 4, 40, 450, DateTimeKind.Local).AddTicks(4837));

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartingDateTime",
                value: new DateTime(2022, 1, 2, 17, 4, 40, 450, DateTimeKind.Local).AddTicks(4915));

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 3,
                column: "StartingDateTime",
                value: new DateTime(2022, 1, 2, 16, 4, 40, 450, DateTimeKind.Local).AddTicks(4922));
        }
    }
}
