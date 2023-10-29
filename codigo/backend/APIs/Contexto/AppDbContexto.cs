using APIs.Modelos;
using APIs.Modelos.Entidade;
using Microsoft.EntityFrameworkCore;

namespace APIs.Contexto;

public class AppDbContexto : DbContext
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Endereco> Enderecos => Set<Endereco>();

    public AppDbContexto(DbContextOptions<AppDbContexto> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Usuario>()
            .HasOne(a => a.EnderecoDoUsuario)
            .WithOne(a => a.UsuarioQuePertenceAEsseEndereco)
            .HasForeignKey<Endereco>(e => e.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Usuario>().HasIndex(e => e.Identificador).IsUnique();
        modelBuilder.Entity<Endereco>().HasIndex(e => e.Cep).IsUnique();
    }
}
