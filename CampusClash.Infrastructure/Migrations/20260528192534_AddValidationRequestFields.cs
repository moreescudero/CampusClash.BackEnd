using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusClash.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddValidationRequestFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Career",
                table: "ValidationRequests",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Faculty",
                table: "ValidationRequests",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Legajo",
                table: "ValidationRequests",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "ValidationRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Career",
                table: "ValidationRequests");

            migrationBuilder.DropColumn(
                name: "Faculty",
                table: "ValidationRequests");

            migrationBuilder.DropColumn(
                name: "Legajo",
                table: "ValidationRequests");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "ValidationRequests");
        }
    }
}
