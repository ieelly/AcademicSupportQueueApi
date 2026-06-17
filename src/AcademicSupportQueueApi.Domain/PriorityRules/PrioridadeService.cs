using AcademicSupportQueueApi.Domain.Enums;

namespace AcademicSupportQueueApi.Domain.PriorityRules;

public class PrioridadeService
{
    public int Calcular(
        TipoSolicitacao tipo,
        DateTime prazoEntrega)
    {
        return PesoTipo(tipo)
             + PesoPrazo(prazoEntrega);
    }

    private int PesoTipo(
        TipoSolicitacao tipo)
    {
        return tipo switch
        {
            TipoSolicitacao.TrabalhoFinal => 50,
            TipoSolicitacao.Prova => 40,
            TipoSolicitacao.Projeto => 30,
            TipoSolicitacao.ListaExercicio => 20,
            TipoSolicitacao.DuvidaGeral => 10,
            _ => 0
        };
    }

    private int PesoPrazo(
        DateTime prazo)
    {
        var dias =
            (prazo.Date - DateTime.Today).Days;

        if (dias <= 0)
            return 50;

        if (dias <= 1)
            return 40;

        if (dias <= 3)
            return 30;

        if (dias <= 7)
            return 20;

        return 10;
    }
}
