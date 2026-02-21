using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LogNow.API.Migrations
{
    /// <inheritdoc />
    public partial class AddNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    RelatedEntityId = table.Column<string>(type: "TEXT", nullable: true),
                    IsRead = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SLAs",
                columns: new[] { "Id", "CreatedAt", "ResolutionTimeMinutes", "ResponseTimeMinutes", "Severity" },
                values: new object[,]
                {
                    { new Guid("0cea91d5-6c8d-4998-80d8-b658bf2caa87"), new DateTime(2026, 2, 21, 4, 20, 12, 120, DateTimeKind.Utc).AddTicks(8863), 1440, 60, "SEV3" },
                    { new Guid("5e17ac8e-b353-49f1-bd15-36d83366bf3b"), new DateTime(2026, 2, 21, 4, 20, 12, 120, DateTimeKind.Utc).AddTicks(8860), 120, 15, "SEV2" },
                    { new Guid("8ac52068-dddc-4c17-8091-318ee49cdaae"), new DateTime(2026, 2, 21, 4, 20, 12, 120, DateTimeKind.Utc).AddTicks(8823), 30, 5, "SEV1" },
                    { new Guid("dc2cb19a-5cbb-497a-a2cb-9e93856fb022"), new DateTime(2026, 2, 21, 4, 20, 12, 120, DateTimeKind.Utc).AddTicks(8865), 4320, 240, "SEV4" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_IsRead",
                table: "Notifications",
                columns: new[] { "UserId", "IsRead" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("0cea91d5-6c8d-4998-80d8-b658bf2caa87"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("5e17ac8e-b353-49f1-bd15-36d83366bf3b"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("8ac52068-dddc-4c17-8091-318ee49cdaae"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("dc2cb19a-5cbb-497a-a2cb-9e93856fb022"));

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
    }
}
