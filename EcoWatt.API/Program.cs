using EcoWatt.API.Data;
using EcoWatt.API.Repositories;
using EcoWatt.API.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Registro do OracleDbContext com a string de conex�o correta
builder.Services.AddDbContext<OracleDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Registro dos reposit�rios e servi�os existentes
builder.Services.AddScoped<IConsumoAgregadoRepository, ConsumoAgregadoRepository>();
builder.Services.AddScoped<IConsumoAgregadoService, ConsumoAgregadoService>();

// Registrar os servi�os de Relat�rio
builder.Services.AddScoped<IRelatorioRepository, RelatorioRepository>();
builder.Services.AddScoped<IRelatorioService, RelatorioService>();

// Adiciona controladores com configura��o para ignorar ciclos
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Configura��es de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configura��es do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();