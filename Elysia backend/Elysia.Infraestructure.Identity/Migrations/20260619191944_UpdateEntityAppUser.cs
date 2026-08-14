using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elysia.Infraestructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntityAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Especialidad",
                schema: "Identity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Especialidad",
                schema: "Identity",
                table: "Users");
        }
    }
}
