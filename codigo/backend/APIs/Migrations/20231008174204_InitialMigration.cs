using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIs.Migrations;

/// <inheritdoc />
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Empresas",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                CNPJ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Senha = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Empresas", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Enderecos",
            columns: table => new
            {
                EnderecoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Rua = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                Numero = table.Column<int>(type: "int", nullable: false),
                Bairro = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                Cidade = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                CEP = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Enderecos", x => x.EnderecoId);
            });

        migrationBuilder.CreateTable(
            name: "Alunos",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                RG = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                CPF = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                EnderecoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                InstituicaoDeEnsino = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Senha = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Alunos", x => x.Id);
                table.ForeignKey(
                    name: "FK_Alunos_Enderecos_EnderecoId",
                    column: x => x.EnderecoId,
                    principalTable: "Enderecos",
                    principalColumn: "EnderecoId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Alunos_EnderecoId",
            table: "Alunos",
            column: "EnderecoId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Alunos");

        migrationBuilder.DropTable(
            name: "Empresas");

        migrationBuilder.DropTable(
            name: "Enderecos");
    }
}
