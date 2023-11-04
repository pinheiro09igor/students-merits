using System.Reflection;
using System.Text.Json.Serialization;
using APIs.Contexto;
using APIs.Modelos.Entidade;
using APIs.Repositorios;
using APIs.Servicos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("local");
const string myCors = "allowAll";
builder.Services.AddCors(options => options.AddPolicy(name: myCors, policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }));
builder.Services.AddDbContext<AppDbContexto>(options => options.UseSqlServer(connectionString));
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioServico>();
builder.Services.AddScoped<IAutenticacaoRepositorio, AutenticacaoServico>();
builder.Services.AddScoped<IBancoRepositorio, BancoServico>();
builder.Services.AddScoped<IVantagemRepositorio, VantagemServico>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
  {
      options.Audience = builder.Configuration["Jwt:ValidAudience"];
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = true,
          ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
          ValidateAudience = true,
          ValidAudience = builder.Configuration["Jwt:ValidAudience"],
          ValidateLifetime = false,
          ValidateIssuerSigningKey = true
      };
  });
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(myCors);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
