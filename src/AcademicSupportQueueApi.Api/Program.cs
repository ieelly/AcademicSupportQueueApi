using AcademicSupportQueueApi.Domain.PriorityRules;
using AcademicSupportQueueApi.Domain.Services;
using AcademicSupportQueueApi.Infrastructure.Dados;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ContextoBanco>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ConexaoPadrao")));

builder.Services.AddSingleton<HeapService>();

builder.Services.AddScoped<PrioridadeService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();