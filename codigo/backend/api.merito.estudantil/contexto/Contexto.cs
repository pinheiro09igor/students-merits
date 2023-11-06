using api.merito.estudantil.models;
using Microsoft.EntityFrameworkCore;

namespace api.merito.estudantil.contexto;

public class Contexto : DbContext
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<Professor> Professores => Set<Professor>();
    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<Vantagem> Vantagens => Set<Vantagem>();
    public DbSet<Transacao> Transacoes => Set<Transacao>();
    
    public Contexto(DbContextOptions options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();
        
        modelBuilder.Entity<Aluno>().HasIndex(a => a.Rg).IsUnique();
        modelBuilder.Entity<Aluno>().HasIndex(a => a.Cpf).IsUnique();
        modelBuilder.Entity<Aluno>().HasIndex(a => a.Email).IsUnique();
        
        modelBuilder.Entity<Professor>().HasIndex(p => p.Email).IsUnique();
        modelBuilder.Entity<Professor>().HasIndex(p => p.Cpf).IsUnique();

        modelBuilder.Entity<Empresa>().HasIndex(e => e.Email).IsUnique();
    }
}