using APIs.Contexto;
using APIs.Interfaces;
using APIs.Servicos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("local");
var myCors = "allowAll";
builder.Services.AddCors(options => options.AddPolicy(name: myCors, policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }));
builder.Services.AddDbContext<AppDbContexto>(options => options.UseSqlServer(connectionString));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAlunoRepositorio, AlunoServico>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(myCors);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
