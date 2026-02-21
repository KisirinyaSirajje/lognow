using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LogNow.API.Migrations
{
    /// <inheritdoc />
    public partial class AddResolutionNoteAndHoldReason : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "OnHoldReason",
                table: "Incidents",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResolutionNote",
                table: "Incidents",
                type: "TEXT",
                nullable: true);

            migrationBuilder.InsertData(
                table: "SLAs",
                columns: new[] { "Id", "CreatedAt", "ResolutionTimeMinutes", "ResponseTimeMinutes", "Severity" },
                values: new object[,]
                {
                    { new Guid("63ffb8ac-2987-45b4-872b-7ce837d3ab75"), new DateTime(2026, 2, 21, 3, 25, 55, 853, DateTimeKind.Utc).AddTicks(1164), 120, 15, "SEV2" },
                    { new Guid("727bee93-794c-4498-b66b-8c7cdf0d54ad"), new DateTime(2026, 2, 21, 3, 25, 55, 853, DateTimeKind.Utc).AddTicks(1177), 1440, 60, "SEV3" },
                    { new Guid("9acc7dac-28f4-4868-be05-4140f04e8d99"), new DateTime(2026, 2, 21, 3, 25, 55, 853, DateTimeKind.Utc).AddTicks(1135), 30, 5, "SEV1" },
                    { new Guid("aac367c7-a923-428a-ac29-1acceb0476b7"), new DateTime(2026, 2, 21, 3, 25, 55, 853, DateTimeKind.Utc).AddTicks(1179), 4320, 240, "SEV4" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("63ffb8ac-2987-45b4-872b-7ce837d3ab75"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("727bee93-794c-4498-b66b-8c7cdf0d54ad"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("9acc7dac-28f4-4868-be05-4140f04e8d99"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("aac367c7-a923-428a-ac29-1acceb0476b7"));

            migrationBuilder.DropColumn(
                name: "OnHoldReason",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "ResolutionNote",
                table: "Incidents");

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
        }
    }
}
