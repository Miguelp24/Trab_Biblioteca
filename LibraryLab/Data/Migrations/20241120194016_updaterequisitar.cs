using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryLab.Data.Migrations
{
    /// <inheritdoc />
    public partial class updaterequisitar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "BibliotecarioRecebeId",
                table: "Requisitar",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requisitar_BibliotecarioRecebeId",
                table: "Requisitar",
                column: "BibliotecarioRecebeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requisitar_Utilizadores_BibliotecarioRecebeId",
                table: "Requisitar",
                column: "BibliotecarioRecebeId",
                principalTable: "Utilizadores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requisitar_Utilizadores_BibliotecarioRecebeId",
                table: "Requisitar");

            migrationBuilder.DropIndex(
                name: "IX_Requisitar_BibliotecarioRecebeId",
                table: "Requisitar");

            migrationBuilder.DropColumn(
                name: "BibliotecarioRecebeId",
                table: "Requisitar");
        }
    }
}
