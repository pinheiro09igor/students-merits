using APIs.Contexto;
using APIs.Interfaces;
using APIs.Servicos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("local");
var myCors = "allowAll";
builder.Services.AddCors(options => options.AddPolicy(name: myCors, policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }));
builder.Services.AddDbContext<AppDbContexto>(options => options.UseSqlServer(connectionString));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAlunoRepositorio, AlunoServico>();
builder.Services.AddScoped<IEmpresaRepositorio, EmpresaServico>();
builder.Services.AddScoped<IAuthRepositorio, AuthServico>();
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
