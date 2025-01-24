using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryLab.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateuserblock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlockReason",
                table: "Utilizadores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "Utilizadores",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockReason",
                table: "Utilizadores");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "Utilizadores");
        }
    }
}
