using AcademicSupportQueueApi.Domain.Entidades;

namespace AcademicSupportQueueApi.Domain.Interfaces;

public interface IAtendimentoRepository
{
    Task<List<Atendimento>> ListarAsync(int page, int size);
    Task<Atendimento?> BuscarPorIdAsync(Guid id);
    Task<List<Atendimento>> BuscarPorDescricaoAsync(string descricao);
    Task CadastrarAsync(Atendimento atendimento);
    Task AtualizarAsync(Atendimento atendimento);
    Task SalvarAsync();
}