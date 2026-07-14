using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elysia.Infraestructure.persistences.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlatoProductoRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatoProductos_Platos_PlatoId",
                table: "PlatoProductos");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatoProductos_Platos_PlatoId",
                table: "PlatoProductos",
                column: "PlatoId",
                principalTable: "Platos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatoProductos_Platos_PlatoId",
                table: "PlatoProductos");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatoProductos_Platos_PlatoId",
                table: "PlatoProductos",
                column: "PlatoId",
                principalTable: "Platos",
                principalColumn: "Id");
        }
    }
}
