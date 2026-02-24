using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LogNow.API.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "WorkOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    WorkOrderNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Priority = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    ServiceId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IncidentId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AssignedToUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AssignedGroup = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ScheduledStartDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ScheduledEndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ActualStartDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ActualEndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EstimatedCost = table.Column<decimal>(type: "TEXT", nullable: true),
                    ActualCost = table.Column<decimal>(type: "TEXT", nullable: true),
                    EstimatedHours = table.Column<decimal>(type: "TEXT", nullable: true),
                    ActualHours = table.Column<decimal>(type: "TEXT", nullable: true),
                    Location = table.Column<string>(type: "TEXT", nullable: true),
                    PartsRequired = table.Column<string>(type: "TEXT", nullable: true),
                    CompletionNotes = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrders_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_WorkOrders_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_WorkOrders_Users_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_WorkOrders_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_AssignedToUserId",
                table: "WorkOrders",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_CreatedByUserId",
                table: "WorkOrders",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_IncidentId",
                table: "WorkOrders",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_ServiceId",
                table: "WorkOrders",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_WorkOrderNumber",
                table: "WorkOrders",
                column: "WorkOrderNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkOrders");

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
        }
    }
}
