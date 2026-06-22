using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusClash.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLcuSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LcuSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    BaseUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    AuthToken = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LcuSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LcuSessions_TournamentMatches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "TournamentMatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LcuSessions_MatchId",
                table: "LcuSessions",
                column: "MatchId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LcuSessions");
        }
    }
}
