using Microsoft.AspNetCore.Mvc;
using PainelIntegraTelefoniaIP.Entity.Interfaces;
using PainelIntegraTelefoniaIP.Models.Interfaces;
using PainelIntegraTelefoniaIP.Services.Interfaces;

namespace PainelIntegraTelefoniaIP.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class MeuControllerBase<TDto, TEntity> : ControllerBase
    where TDto: class, IMeusDtos
    where TEntity : class, IEntity
{
    private readonly IService<TDto,TEntity> _service;
    public MeuControllerBase(IService<TDto, TEntity> service)
    {
        _service = service;
    }
    [HttpGet("Listar")]
    public ActionResult<IEnumerable<IMeusDtos>> ListarTudo()
    {
        var lista = _service.ListarTodos();
        return Ok(lista);
    }
    [HttpGet("Buscar/{id}")]
    public ActionResult<IMeusDtos> BuscarPorId(int id)
    {
        var dto = _service.BuscarPorId(id);
        if (dto == null)
            return NotFound();
        return Ok(dto);
    }
    [HttpPost("Criar")]
    public ActionResult Criar([FromBody] TDto dto)
    {
        _service.Criar(dto);
        return Ok();
    }
    [HttpPut("Editar/{id}")]
    public ActionResult Editar([FromBody] TDto dto, int id)
    {
        _service.Editar(id, dto);
        return NoContent();
    }
    [HttpDelete("Deletar/{id}")]
    public ActionResult Deletar(int id)
    {
        _service.Excluir(id);
        return NoContent();
    }
}