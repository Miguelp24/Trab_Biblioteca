using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryLab.Data.Migrations
{
    /// <inheritdoc />
    public partial class addrequisitar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requisitar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BibliotecarioId = table.Column<int>(type: "int", nullable: true),
                    LeitorId = table.Column<int>(type: "int", nullable: false),
                    LivroId = table.Column<int>(type: "int", nullable: false),
                    DataRequisicao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataEmprestimo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataDevolucao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requisitar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requisitar_Livro_LivroId",
                        column: x => x.LivroId,
                        principalTable: "Livro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requisitar_Utilizadores_BibliotecarioId",
                        column: x => x.BibliotecarioId,
                        principalTable: "Utilizadores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requisitar_Utilizadores_LeitorId",
                        column: x => x.LeitorId,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requisitar_BibliotecarioId",
                table: "Requisitar",
                column: "BibliotecarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitar_LeitorId",
                table: "Requisitar",
                column: "LeitorId");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitar_LivroId",
                table: "Requisitar",
                column: "LivroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requisitar");
        }
    }
}
