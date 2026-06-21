using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusClash.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduleAndRiotLobby : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RiotTournamentId",
                table: "Tournaments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RiotLobbyCode",
                table: "TournamentMatches",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledAt",
                table: "TournamentMatches",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RiotTournamentId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "RiotLobbyCode",
                table: "TournamentMatches");

            migrationBuilder.DropColumn(
                name: "ScheduledAt",
                table: "TournamentMatches");
        }
    }
}
