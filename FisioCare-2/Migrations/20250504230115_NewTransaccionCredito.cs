using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FisioCare_2.Migrations
{
    /// <inheritdoc />
    public partial class NewTransaccionCredito : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreditosDisponibles",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TransaccionCredito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaqueteCreditoId = table.Column<int>(type: "int", nullable: false),
                    CantidadCreditos = table.Column<int>(type: "int", nullable: false),
                    FechaTransaccion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransaccionCredito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransaccionCredito_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransaccionCredito_PaquetesCredito_PaqueteCreditoId",
                        column: x => x.PaqueteCreditoId,
                        principalTable: "PaquetesCredito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransaccionCredito_ApplicationUserId",
                table: "TransaccionCredito",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransaccionCredito_PaqueteCreditoId",
                table: "TransaccionCredito",
                column: "PaqueteCreditoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransaccionCredito");

            migrationBuilder.DropColumn(
                name: "CreditosDisponibles",
                table: "AspNetUsers");
        }
    }
}
