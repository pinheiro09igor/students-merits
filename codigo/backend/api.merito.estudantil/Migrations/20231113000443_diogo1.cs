using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.merito.estudantil.Migrations
{
    /// <inheritdoc />
    public partial class diogo1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResgatadaPor",
                table: "Vantagens",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResgatadaPor",
                table: "Vantagens");
        }
    }
}
