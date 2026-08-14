using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elysia.Infraestructure.persistences.Migrations
{
    /// <inheritdoc />
    public partial class MigrationUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatoProductos_Productos_PlatoId",
                table: "PlatoProductos");

            migrationBuilder.CreateIndex(
                name: "IX_PlatoProductos_ProductoId",
                table: "PlatoProductos",
                column: "ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatoProductos_Productos_ProductoId",
                table: "PlatoProductos",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatoProductos_Productos_ProductoId",
                table: "PlatoProductos");

            migrationBuilder.DropIndex(
                name: "IX_PlatoProductos_ProductoId",
                table: "PlatoProductos");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatoProductos_Productos_PlatoId",
                table: "PlatoProductos",
                column: "PlatoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
