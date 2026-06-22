using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusClash.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLcuSessionLobbyCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LobbyCreated",
                table: "LcuSessions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LobbyCreated",
                table: "LcuSessions");
        }
    }
}
