using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryLab.Data.Migrations
{
    /// <inheritdoc />
    public partial class addestadoaolivro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "role",
                table: "Utilizadores",
                newName: "Role");

            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "Livro",
                type: "bit",
                nullable: false,
                defaultValue: false);

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Livro");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Utilizadores",
                newName: "role");
        }
    }
}
