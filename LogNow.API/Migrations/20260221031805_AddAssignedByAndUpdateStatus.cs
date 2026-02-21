using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LogNow.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedByAndUpdateStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("0546e115-2015-4522-a4ca-0259389f0865"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("29f55da7-2fc3-4371-a5e7-c11d993161d8"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("53c91854-8b15-4cf1-aadd-b4f352c2ef1c"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("978f6b2c-b69e-42b1-a8eb-21e4d0254128"));

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedByUserId",
                table: "Incidents",
                type: "TEXT",
                nullable: true);

            migrationBuilder.InsertData(
                table: "SLAs",
                columns: new[] { "Id", "CreatedAt", "ResolutionTimeMinutes", "ResponseTimeMinutes", "Severity" },
                values: new object[,]
                {
                    { new Guid("05ead88a-5f12-48ce-8394-42cc9241effc"), new DateTime(2026, 2, 21, 3, 18, 5, 422, DateTimeKind.Utc).AddTicks(650), 30, 5, "SEV1" },
                    { new Guid("20bf742f-13e3-4afe-81aa-cbd0b24ac1e9"), new DateTime(2026, 2, 21, 3, 18, 5, 422, DateTimeKind.Utc).AddTicks(689), 4320, 240, "SEV4" },
                    { new Guid("49bc0570-092a-48fb-a9ad-c817c7f88fea"), new DateTime(2026, 2, 21, 3, 18, 5, 422, DateTimeKind.Utc).AddTicks(686), 1440, 60, "SEV3" },
                    { new Guid("9180b155-d27d-4d74-9eb7-c82b1110af1a"), new DateTime(2026, 2, 21, 3, 18, 5, 422, DateTimeKind.Utc).AddTicks(683), 120, 15, "SEV2" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_AssignedByUserId",
                table: "Incidents",
                column: "AssignedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Users_AssignedByUserId",
                table: "Incidents",
                column: "AssignedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Users_AssignedByUserId",
                table: "Incidents");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_AssignedByUserId",
                table: "Incidents");

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("05ead88a-5f12-48ce-8394-42cc9241effc"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("20bf742f-13e3-4afe-81aa-cbd0b24ac1e9"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("49bc0570-092a-48fb-a9ad-c817c7f88fea"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("9180b155-d27d-4d74-9eb7-c82b1110af1a"));

            migrationBuilder.DropColumn(
                name: "AssignedByUserId",
                table: "Incidents");

            migrationBuilder.InsertData(
                table: "SLAs",
                columns: new[] { "Id", "CreatedAt", "ResolutionTimeMinutes", "ResponseTimeMinutes", "Severity" },
                values: new object[,]
                {
                    { new Guid("0546e115-2015-4522-a4ca-0259389f0865"), new DateTime(2026, 2, 21, 3, 0, 50, 856, DateTimeKind.Utc).AddTicks(7270), 1440, 60, "SEV3" },
                    { new Guid("29f55da7-2fc3-4371-a5e7-c11d993161d8"), new DateTime(2026, 2, 21, 3, 0, 50, 856, DateTimeKind.Utc).AddTicks(7272), 4320, 240, "SEV4" },
                    { new Guid("53c91854-8b15-4cf1-aadd-b4f352c2ef1c"), new DateTime(2026, 2, 21, 3, 0, 50, 856, DateTimeKind.Utc).AddTicks(7216), 30, 5, "SEV1" },
                    { new Guid("978f6b2c-b69e-42b1-a8eb-21e4d0254128"), new DateTime(2026, 2, 21, 3, 0, 50, 856, DateTimeKind.Utc).AddTicks(7267), 120, 15, "SEV2" }
                });
        }
    }
}
