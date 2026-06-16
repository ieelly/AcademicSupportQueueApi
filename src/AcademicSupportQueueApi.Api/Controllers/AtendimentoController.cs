using AcademicSupportQueueApi.Domain.Entidades;
using AcademicSupportQueueApi.Infrastructure.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademicSupportQueueApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AtendimentoController : ControllerBase
{
    private readonly ContextoBanco _contexto;

    public AtendimentoController(ContextoBanco contexto)
    {
        _contexto = contexto;
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
            .FirstOrDefaultAsync(a => a.Id == id && a.Status != "Excluido");

        if (atendimento == null)
            return NotFound("Atendimento não encontrado.");

        return Ok(atendimento);
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar(Atendimento atendimento)
    {
        atendimento.Id = Guid.NewGuid();
        atendimento.DataCriacao = DateTime.Now;
        atendimento.Status = "Aguardando";

        _contexto.Atendimentos.Add(atendimento);
        await _contexto.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = atendimento.Id }, atendimento);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(Guid id, Atendimento dados)
    {
        var atendimento = await _contexto.Atendimentos.FindAsync(id);

        if (atendimento == null || atendimento.Status == "Excluido")
            return NotFound("Atendimento não encontrado.");

        atendimento.NomeAluno = dados.NomeAluno;
        atendimento.Prioridade = dados.Prioridade;
        atendimento.DataAtualizacao = DateTime.Now;

        await _contexto.SaveChangesAsync();

        return Ok(atendimento);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var atendimento = await _contexto.Atendimentos.FindAsync(id);

        if (atendimento == null || atendimento.Status == "Excluido")
            return NotFound("Atendimento não encontrado.");

        atendimento.Status = "Excluido";
        atendimento.DataExclusao = DateTime.Now;

        await _contexto.SaveChangesAsync();

        return NoContent();
    }
}