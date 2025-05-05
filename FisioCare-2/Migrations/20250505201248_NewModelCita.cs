using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FisioCare_2.Migrations
{
    /// <inheritdoc />
    public partial class NewModelCita : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cita",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FisioterapeutaId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ServicioId = table.Column<int>(type: "int", nullable: true),
                    HoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notas = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cita", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cita_AspNetUsers_FisioterapeutaId",
                        column: x => x.FisioterapeutaId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cita_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cita_Servicio_ServicioId",
                        column: x => x.ServicioId,
                        principalTable: "Servicio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cita_FisioterapeutaId",
                table: "Cita",
                column: "FisioterapeutaId");

            migrationBuilder.CreateIndex(
                name: "IX_Cita_ServicioId",
                table: "Cita",
                column: "ServicioId");

            migrationBuilder.CreateIndex(
                name: "IX_Cita_UsuarioId",
                table: "Cita",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cita");
        }
    }
}
