using AcademicSupportQueueApi.Domain.Enums;

namespace AcademicSupportQueueApi.Domain.Entities;

public class Atendimento
{
    public Guid Id { get; set; }

    public string NomeAluno { get; set; } = string.Empty;

    public string Cpf { get; set; } = string.Empty;

    public string Disciplina { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public TipoSolicitacao TipoSolicitacao { get; set; }

    public DateTime DataEntrega { get; set; }

    public int Prioridade { get; set; }

    public string Status { get; set; } = "Aguardando";

    public DateTime DataCriacao { get; set; } = DateTime.Now;

    public DateTime? DataAtualizacao { get; set; }

    public DateTime? DataExclusao { get; set; }
}