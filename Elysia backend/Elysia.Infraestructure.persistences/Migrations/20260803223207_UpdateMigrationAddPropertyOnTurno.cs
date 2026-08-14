using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elysia.Infraestructure.persistences.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigrationAddPropertyOnTurno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PropietarioId",
                table: "Shifts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropietarioId",
                table: "Shifts");
        }
    }
}
