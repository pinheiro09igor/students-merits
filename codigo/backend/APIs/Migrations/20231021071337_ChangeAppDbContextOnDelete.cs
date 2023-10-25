using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIs.Migrations;

/// <inheritdoc />
public partial class ChangeAppDbContextOnDelete : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Enderecos_Alunos_AlunoRef",
            table: "Enderecos");

        migrationBuilder.AddForeignKey(
            name: "FK_Enderecos_Alunos_AlunoRef",
            table: "Enderecos",
            column: "AlunoRef",
            principalTable: "Alunos",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Enderecos_Alunos_AlunoRef",
            table: "Enderecos");

        migrationBuilder.AddForeignKey(
            name: "FK_Enderecos_Alunos_AlunoRef",
            table: "Enderecos",
            column: "AlunoRef",
            principalTable: "Alunos",
            principalColumn: "Id");
    }
}
