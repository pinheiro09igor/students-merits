using APIs.Modelos;
using Microsoft.EntityFrameworkCore;

namespace APIs.Contexto;

public class AppDbContexto : DbContext
{
    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<Endereco> Enderecos => Set<Endereco>();
    public DbSet<Empresa> Empresas => Set<Empresa>();

    public AppDbContexto(DbContextOptions<AppDbContexto> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Aluno>()
            .HasOne(a => a.Endereco)
            .WithOne(a => a.Aluno)
            .HasForeignKey<Endereco>(e => e.AlunoRef)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
