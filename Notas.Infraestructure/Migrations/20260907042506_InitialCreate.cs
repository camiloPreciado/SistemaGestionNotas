using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notas.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdEstudiante = table.Column<int>(type: "int", nullable: false),
                    IdProfesor = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notas_IdEstudiante",
                table: "Notas",
                column: "IdEstudiante");

            migrationBuilder.CreateIndex(
                name: "IX_Notas_IdProfesor",
                table: "Notas",
                column: "IdProfesor");

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_Estudiantes_IdEstudiante",
                table: "Notas",
                column: "IdEstudiante",
                principalTable: "Estudiantes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_Profesores_IdProfesor",
                table: "Notas",
                column: "IdProfesor",
                principalTable: "Profesores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notas");
        }
    }
}
