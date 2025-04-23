using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FisioCare_2.Migrations
{
    /// <inheritdoc />
    public partial class MakeUsuarioIdNullableInHorario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Horarios_AspNetUsers_UsuarioId",
                table: "Horarios");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Horarios",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Horarios_AspNetUsers_UsuarioId",
                table: "Horarios",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Horarios_AspNetUsers_UsuarioId",
                table: "Horarios");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Horarios",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Horarios_AspNetUsers_UsuarioId",
                table: "Horarios",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
