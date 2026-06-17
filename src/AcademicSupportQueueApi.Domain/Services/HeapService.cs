using AcademicSupportQueueApi.Domain.Entidades;

namespace AcademicSupportQueueApi.Domain.Services;

public class HeapService
{
    private readonly PriorityQueue<Atendimento, int> _fila = new();

    public void Adicionar(Atendimento atendimento)
    {
        _fila.Enqueue(
            atendimento,
            -atendimento.Prioridade);
    }

    public Atendimento? ConsultarProximo()
    {
        _fila.TryPeek(
            out var atendimento,
            out _);

        return atendimento;
    }

    public Atendimento? AtenderProximo()
    {
        if (_fila.Count == 0)
            return null;

        return _fila.Dequeue();
    }

    public int TotalItens()
    {
        return _fila.Count;
    }
}