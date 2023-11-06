using api.merito.estudantil.contexto;
using api.merito.estudantil.models;
using api.merito.estudantil.repositorios;
using api.merito.estudantil.servicos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["ConnectionStrings:local"];
builder.Services.AddDbContext<Contexto>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IEmail, EmailServico>();
builder.Services.AddScoped<IRepositorioTransacao, TransacaoServico>();
builder.Services.AddScoped<IRepositorioResgateVantagem, ResgateVantagemServico>();
builder.Services.AddScoped<IRepositorioTransferenciaDeMoedas, TransferirMoedasServico>();
builder.Services.AddScoped<IRepositorioUsuario, UsuarioServico>();
builder.Services.AddScoped<IRepositorioGenerico<Aluno>, AlunoServico>();
builder.Services.AddScoped<IRepositorioGenerico<Professor>, ProfessorServico>();
builder.Services.AddScoped<IRepositorioGenerico<Empresa>, EmpresaServico>();
builder.Services.AddScoped<IRepositorioVantagem, VantagemServico>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();