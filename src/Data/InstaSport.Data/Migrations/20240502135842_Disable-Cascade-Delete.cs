using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstaSport.Data.Migrations
{
    public partial class DisableCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Users_AuthorId",
                table: "Rating");

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

            migrationBuilder.CreateIndex(
                name: "IX_Rating_UserId",
                table: "Rating",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Users_AuthorId",
                table: "Rating",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Users_UserId",
                table: "Rating",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Users_AuthorId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Users_UserId",
                table: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Rating_UserId",
                table: "Rating");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Users_AuthorId",
                table: "Rating",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
