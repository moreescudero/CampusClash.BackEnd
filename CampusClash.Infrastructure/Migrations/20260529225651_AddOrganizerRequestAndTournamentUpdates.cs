using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusClash.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizerRequestAndTournamentUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOrganizer",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Game",
                table: "Tournaments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EnrollmentDeadline",
                table: "Tournaments",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Tournaments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsInterUniversity",
                table: "Tournaments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UniversityId",
                table: "Tournaments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrganizerRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserEmail = table.Column<string>(type: "text", nullable: false),
                    TournamentName = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Game = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsInterUniversity = table.Column<bool>(type: "boolean", nullable: false),
                    UniversityId = table.Column<Guid>(type: "uuid", nullable: true),
                    MaxTeams = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizerRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizerRequests_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganizerRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_UniversityId",
                table: "Tournaments",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizerRequests_UniversityId",
                table: "OrganizerRequests",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizerRequests_UserId",
                table: "OrganizerRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Universities_UniversityId",
                table: "Tournaments",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Universities_UniversityId",
                table: "Tournaments");

            migrationBuilder.DropTable(
                name: "OrganizerRequests");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_UniversityId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "IsOrganizer",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "IsInterUniversity",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "Tournaments");

            migrationBuilder.AlterColumn<string>(
                name: "Game",
                table: "Tournaments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EnrollmentDeadline",
                table: "Tournaments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
