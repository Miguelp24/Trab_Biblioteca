using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryLab.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLivro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Livro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ISBN = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AutorId = table.Column<int>(type: "int", nullable: false),
                    GeneroId = table.Column<int>(type: "int", nullable: false),
                    Num_exemplares = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Livro_Autor_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Autor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Livro_Genero_GeneroId",
                        column: x => x.GeneroId,
                        principalTable: "Genero",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Livro_AutorId",
                table: "Livro",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Livro_GeneroId",
                table: "Livro",
                column: "GeneroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Livro");
        }
    }
}
