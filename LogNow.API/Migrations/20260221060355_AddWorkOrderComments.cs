using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LogNow.API.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkOrderComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("a89afe8e-dae6-4415-88fa-5e4eeb4e5869"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("c5ababa9-db04-474a-a119-7d7d82c17702"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("d082c02c-f24c-4280-94e1-96cbe3b924f6"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("f36c95f0-67f6-475a-b52f-783a2be6a4bc"));

            migrationBuilder.CreateTable(
                name: "WorkOrderComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CommentText = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrderComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrderComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkOrderComments_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SLAs",
                columns: new[] { "Id", "CreatedAt", "ResolutionTimeMinutes", "ResponseTimeMinutes", "Severity" },
                values: new object[,]
                {
                    { new Guid("8a070d3f-3fe6-4069-ae3f-9ddbe9a74c19"), new DateTime(2026, 2, 21, 6, 3, 54, 17, DateTimeKind.Utc).AddTicks(9783), 4320, 240, "SEV4" },
                    { new Guid("9fe062a3-67c1-4fda-bab6-90296e2ce913"), new DateTime(2026, 2, 21, 6, 3, 54, 17, DateTimeKind.Utc).AddTicks(9753), 1440, 60, "SEV3" },
                    { new Guid("dcd6a24b-c47e-4c47-b549-bbca7a4f27ca"), new DateTime(2026, 2, 21, 6, 3, 54, 17, DateTimeKind.Utc).AddTicks(9454), 30, 5, "SEV1" },
                    { new Guid("fc6cfce8-39cf-4c0a-9c36-73feef2fd621"), new DateTime(2026, 2, 21, 6, 3, 54, 17, DateTimeKind.Utc).AddTicks(9716), 120, 15, "SEV2" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderComments_UserId",
                table: "WorkOrderComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderComments_WorkOrderId",
                table: "WorkOrderComments",
                column: "WorkOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkOrderComments");

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("8a070d3f-3fe6-4069-ae3f-9ddbe9a74c19"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("9fe062a3-67c1-4fda-bab6-90296e2ce913"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("dcd6a24b-c47e-4c47-b549-bbca7a4f27ca"));

            migrationBuilder.DeleteData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: new Guid("fc6cfce8-39cf-4c0a-9c36-73feef2fd621"));

            migrationBuilder.InsertData(
                table: "SLAs",
                columns: new[] { "Id", "CreatedAt", "ResolutionTimeMinutes", "ResponseTimeMinutes", "Severity" },
                values: new object[,]
                {
                    { new Guid("a89afe8e-dae6-4415-88fa-5e4eeb4e5869"), new DateTime(2026, 2, 21, 5, 3, 52, 259, DateTimeKind.Utc).AddTicks(2655), 120, 15, "SEV2" },
                    { new Guid("c5ababa9-db04-474a-a119-7d7d82c17702"), new DateTime(2026, 2, 21, 5, 3, 52, 259, DateTimeKind.Utc).AddTicks(2621), 30, 5, "SEV1" },
                    { new Guid("d082c02c-f24c-4280-94e1-96cbe3b924f6"), new DateTime(2026, 2, 21, 5, 3, 52, 259, DateTimeKind.Utc).AddTicks(2657), 1440, 60, "SEV3" },
                    { new Guid("f36c95f0-67f6-475a-b52f-783a2be6a4bc"), new DateTime(2026, 2, 21, 5, 3, 52, 259, DateTimeKind.Utc).AddTicks(2659), 4320, 240, "SEV4" }
                });
        }
    }
}
