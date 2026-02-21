using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LogNow.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedGroupToIncident : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "AssignedGroup",
                table: "Incidents",
                type: "TEXT",
                nullable: true);

            migrationBuilder.InsertData(
                table: "SLAs",
                columns: new[] { "Id", "CreatedAt", "ResolutionTimeMinutes", "ResponseTimeMinutes", "Severity" },
                values: new object[,]
                {
                    { new Guid("12d3980c-8a2c-4d85-a629-d756208289a1"), new DateTime(2026, 2, 21, 3, 34, 46, 437, DateTimeKind.Utc).AddTicks(921), 30, 5, "SEV1" },
                    { new Guid("60c1315d-084f-4e5d-9a51-fae5227772d4"), new DateTime(2026, 2, 21, 3, 34, 46, 437, DateTimeKind.Utc).AddTicks(974), 4320, 240, "SEV4" },
                    { new Guid("65453d1c-afb2-4272-ad2a-0b26687395ba"), new DateTime(2026, 2, 21, 3, 34, 46, 437, DateTimeKind.Utc).AddTicks(972), 1440, 60, "SEV3" },
                    { new Guid("6d8bf9e1-66f6-4201-a264-b01a718dff0d"), new DateTime(2026, 2, 21, 3, 34, 46, 437, DateTimeKind.Utc).AddTicks(958), 120, 15, "SEV2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("12d3980c-8a2c-4d85-a629-d756208289a1"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("60c1315d-084f-4e5d-9a51-fae5227772d4"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("65453d1c-afb2-4272-ad2a-0b26687395ba"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("6d8bf9e1-66f6-4201-a264-b01a718dff0d"));

            migrationBuilder.DropColumn(
                name: "AssignedGroup",
                table: "Incidents");

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
    }
}
