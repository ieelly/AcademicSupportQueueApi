using AcademicSupportQueueApi.Domain.Entities;
using AcademicSupportQueueApi.Domain.PriorityRules;
using AcademicSupportQueueApi.Domain.Services;
using AcademicSupportQueueApi.Infrastructure.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademicSupportQueueApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AtendimentoController : ControllerBase
{
    private readonly ContextoBanco _contexto;
    private readonly PrioridadeService _prioridadeService;
    private readonly HeapService _heapService;

    public AtendimentoController(
        ContextoBanco contexto,
        PrioridadeService prioridadeService,
        HeapService heapService)
    {
        _contexto = contexto;
        _prioridadeService = prioridadeService;
        _heapService = heapService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var atendimentos = await _contexto.Atendimentos
            .Where(a => a.Status != "Excluido")
            .ToListAsync();

        return Ok(atendimentos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var atendimento = await _contexto.Atendimentos
            .FirstOrDefaultAsync(a =>
                a.Id == id &&
                a.Status != "Excluido");

        if (atendimento == null)
            return NotFound("Atendimento não encontrado.");

        return Ok(atendimento);
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar(
        string? cpf,
        string? descricao)
    {
        var query = _contexto.Atendimentos
            .Where(a => a.Status != "Excluido");

        if (!string.IsNullOrWhiteSpace(cpf))
        {
            query = query.Where(a =>
                a.Cpf.Contains(cpf));
        }

        if (!string.IsNullOrWhiteSpace(descricao))
        {
            query = query.Where(a =>
                a.Descricao.Contains(descricao));
        }

        var resultado = await query.ToListAsync();

        return Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar(
        Atendimento atendimento)
    {
        atendimento.Id = Guid.NewGuid();
        atendimento.DataCriacao = DateTime.Now;
        atendimento.Status = "Aguardando";

        atendimento.Prioridade =
            _prioridadeService.Calcular(
                atendimento.TipoSolicitacao,
                atendimento.DataEntrega);

        _heapService.Adicionar(atendimento);

        _contexto.Atendimentos.Add(atendimento);

        await _contexto.SaveChangesAsync();

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = atendimento.Id },
            atendimento);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(
        Guid id,
        Atendimento dados)
    {
        var atendimento =
            await _contexto.Atendimentos.FindAsync(id);

        if (atendimento == null ||
            atendimento.Status == "Excluido")
        {
            return NotFound("Atendimento não encontrado.");
        }

        atendimento.NomeAluno = dados.NomeAluno;
        atendimento.Cpf = dados.Cpf;
        atendimento.Disciplina = dados.Disciplina;
        atendimento.Descricao = dados.Descricao;
        atendimento.TipoSolicitacao = dados.TipoSolicitacao;
        atendimento.DataEntrega = dados.DataEntrega;

        atendimento.Prioridade =
            _prioridadeService.Calcular(
                atendimento.TipoSolicitacao,
                atendimento.DataEntrega);

        atendimento.DataAtualizacao = DateTime.Now;

        await _contexto.SaveChangesAsync();

        return Ok(atendimento);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var atendimento =
            await _contexto.Atendimentos.FindAsync(id);

        if (atendimento == null ||
            atendimento.Status == "Excluido")
        {
            return NotFound("Atendimento não encontrado.");
        }

        atendimento.Status = "Excluido";
        atendimento.DataExclusao = DateTime.Now;

        await _contexto.SaveChangesAsync();

        return NoContent();
    }
}