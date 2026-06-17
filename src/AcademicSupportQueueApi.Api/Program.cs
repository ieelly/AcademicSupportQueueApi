using AcademicSupportQueueApi.Domain.Interfaces;
using AcademicSupportQueueApi.Infrastructure.Repositories;
using AcademicSupportQueueApi.Domain.PriorityRules;
using AcademicSupportQueueApi.Domain.Services;
using AcademicSupportQueueApi.Infrastructure.Dados;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ContextoBanco>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ConexaoPadrao")));

builder.Services.AddScoped<IAtendimentoRepository, AtendimentoRepository>();
builder.Services.AddSingleton<HeapService>();

builder.Services.AddScoped<PrioridadeService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "AcademicSupportQueueApi",
        Version = "v1",
        Description = "API para gerenciamento de solicitações acadêmicas usando fila de prioridade, banco de dados e Docker."
    });
});
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();