using AcademicSupportQueueApi.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace AcademicSupportQueueApi.Infrastructure.Dados;

public class ContextoBanco : DbContext
{
    public ContextoBanco(DbContextOptions<ContextoBanco> options)
        : base(options)
    {
    }

    public DbSet<Atendimento> Atendimentos { get; set; }
}