using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIs.Migrations
{
    /// <inheritdoc />
    public partial class MigrationsVantagem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vantagens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Valor = table.Column<double>(type: "float", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vantagens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VantagensAlunos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdentificadorAluno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomeDaVantagem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValorDaVantagem = table.Column<double>(type: "float", nullable: false),
                    ObtidaEm = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VantagensAlunos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vantagens_Nome",
                table: "Vantagens",
                column: "Nome",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vantagens");

            migrationBuilder.DropTable(
                name: "VantagensAlunos");
        }
    }
}
