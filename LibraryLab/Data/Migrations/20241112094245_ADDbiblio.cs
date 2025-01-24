using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryLab.Data.Migrations
{
    /// <inheritdoc />
    public partial class ADDbiblio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TabelaBiblio",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Localizacao = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Telefone = table.Column<int>(type: "int", nullable: false),
                    Horario_abertura = table.Column<TimeOnly>(type: "time", nullable: false),
                    Horario_fecho = table.Column<TimeOnly>(type: "time", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabelaBiblio", x => x.Name);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TabelaBiblio");
        }
    }
}
