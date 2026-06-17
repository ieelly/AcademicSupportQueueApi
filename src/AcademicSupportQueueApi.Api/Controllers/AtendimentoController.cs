using AcademicSupportQueueApi.Domain.Entidades;
using AcademicSupportQueueApi.Domain.Interfaces;
using AcademicSupportQueueApi.Domain.PriorityRules;
using AcademicSupportQueueApi.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcademicSupportQueueApi.API.Controllers;

[ApiController]
[Route("solicitacoes-academicas")]
public class AtendimentoController : ControllerBase
{
    private readonly IAtendimentoRepository _repositorio;
    private readonly PrioridadeService _prioridadeService;
    private readonly HeapService _heapService;

    public AtendimentoController(
     IAtendimentoRepository repositorio,
     PrioridadeService prioridadeService,
     HeapService heapService)
    {
        _repositorio = repositorio;
        _prioridadeService = prioridadeService;
        _heapService = heapService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var atendimentos = await _repositorio.ListarAsync(page, size);
        return Ok(atendimentos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var atendimento = await _repositorio.BuscarPorIdAsync(id);

        if (atendimento == null)
            return NotFound("Atendimento não encontrado.");

        return Ok(atendimento);
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar([FromQuery] string? descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            return BadRequest("Informe uma descrição para buscar.");

        var resultado = await _repositorio.BuscarPorDescricaoAsync(descricao);
        return Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar(Atendimento atendimento)
    {
        atendimento.Id = Guid.NewGuid();
        atendimento.DataCriacao = DateTime.Now;
        atendimento.Status = "Aguardando";

        atendimento.Prioridade =
            _prioridadeService.Calcular(atendimento.TipoSolicitacao, atendimento.DataEntrega);

        _heapService.Adicionar(atendimento);

        await _repositorio.CadastrarAsync(atendimento);
        await _repositorio.SalvarAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = atendimento.Id }, atendimento);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(Guid id, Atendimento dados)
    {
        var atendimento = await _repositorio.BuscarPorIdAsync(id);

        if (atendimento == null)
            return NotFound("Atendimento não encontrado.");

        atendimento.NomeAluno = dados.NomeAluno;
        atendimento.Cpf = dados.Cpf;
        atendimento.Disciplina = dados.Disciplina;
        atendimento.Descricao = dados.Descricao;
        atendimento.TipoSolicitacao = dados.TipoSolicitacao;
        atendimento.DataEntrega = dados.DataEntrega;
        atendimento.Prioridade =
            _prioridadeService.Calcular(atendimento.TipoSolicitacao, atendimento.DataEntrega);
        atendimento.DataAtualizacao = DateTime.Now;

        await _repositorio.AtualizarAsync(atendimento);
        await _repositorio.SalvarAsync();

        return Ok(atendimento);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var atendimento = await _repositorio.BuscarPorIdAsync(id);

        if (atendimento == null)
            return NotFound("Atendimento não encontrado.");

        atendimento.Status = "Excluido";
        atendimento.DataExclusao = DateTime.Now;
        atendimento.DataAtualizacao = DateTime.Now;

        await _repositorio.AtualizarAsync(atendimento);
        await _repositorio.SalvarAsync();

        return NoContent();
    }
}