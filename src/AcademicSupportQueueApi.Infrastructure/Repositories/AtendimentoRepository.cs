using AcademicSupportQueueApi.Domain.Entidades;
using AcademicSupportQueueApi.Domain.Interfaces;
using AcademicSupportQueueApi.Infrastructure.Dados;
using Microsoft.EntityFrameworkCore;

namespace AcademicSupportQueueApi.Infrastructure.Repositories;

public class AtendimentoRepository : IAtendimentoRepository
{
    private readonly ContextoBanco _contexto;

    public AtendimentoRepository(ContextoBanco contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<Atendimento>> ListarAsync(int page, int size)
    {
        return await _contexto.Atendimentos
            .Where(a => a.Status != "Excluido")
            .OrderByDescending(a => a.Prioridade)
            .ThenBy(a => a.DataCriacao)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
    }

    public async Task<Atendimento?> BuscarPorIdAsync(Guid id)
    {
        return await _contexto.Atendimentos
            .FirstOrDefaultAsync(a => a.Id == id && a.Status != "Excluido");
    }

    public async Task<List<Atendimento>> BuscarPorDescricaoAsync(string descricao)
    {
        return await _contexto.Atendimentos
            .Where(a => a.Status != "Excluido" && a.Descricao.Contains(descricao))
            .ToListAsync();
    }

    public async Task CadastrarAsync(Atendimento atendimento)
    {
        await _contexto.Atendimentos.AddAsync(atendimento);
    }

    public Task AtualizarAsync(Atendimento atendimento)
    {
        _contexto.Atendimentos.Update(atendimento);
        return Task.CompletedTask;
    }

    public async Task SalvarAsync()
    {
        await _contexto.SaveChangesAsync();
    }
}