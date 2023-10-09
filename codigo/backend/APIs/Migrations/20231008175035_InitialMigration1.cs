using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIs.Migrations;

/// <inheritdoc />
public partial class InitialMigration1 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "CNPJ",
            table: "Empresas",
            type: "nvarchar(14)",
            maxLength: 14,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "CNPJ",
            table: "Empresas",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(14)",
            oldMaxLength: 14,
            oldNullable: true);
    }
}
