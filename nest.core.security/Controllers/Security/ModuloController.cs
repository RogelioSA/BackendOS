using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.security.Modulos.Commands;
using nest.core.aplicacion.security.Modulos.Queries;
using nest.core.dominio;
using nest.core.dominio.Aplicacion.Modulo;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Security;

[Authorize]
[ApiController]
[ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.SECURITY)]
[Route($"{ConfigureEndpoints.EndpointsEnum.SECURITY}/[controller]")]
public class ModuloController : Controller
{
    private readonly ISender sender;

    public ModuloController(ISender sender)
    {
        this.sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Modulo>), 200)]
    [ProducesResponseType(typeof(ErrorMessage), 400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<List<Modulo>>> ObtenerTodos(CancellationToken ct)
    {
        var data = await sender.Send(new ObtenerModulosQuery(), ct);
        return Ok(data);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Modulo), 200)]
    [ProducesResponseType(typeof(ErrorMessage), 400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<Modulo>> ObtenerPorId(int id, CancellationToken ct)
    {
        var data = await sender.Send(new ObtenerModuloPorIdQuery(id), ct);
        return Ok(data);
    }

    [HttpPost("filter")]
    [ProducesResponseType(typeof(LoadResult), 200)]
    [ProducesResponseType(typeof(ErrorMessage), 400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<LoadResult>> ObtenerPorFiltro([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
    {
        var data = await sender.Send(new ObtenerModulosPorFiltroDataSourceQuery(options), ct);
        return Ok(data);
    }

    [HttpPost("filter_activos")]
    [ProducesResponseType(typeof(LoadResult), 200)]
    [ProducesResponseType(typeof(ErrorMessage), 400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<LoadResult>> ObtenerPorFiltroActivos([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
    {
        var data = await sender.Send(new ObtenerModulosPorFiltroActivosQuery(options), ct);
        return Ok(data);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Modulo), 200)]
    [ProducesResponseType(typeof(ErrorMessage), 400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<Modulo>> Agregar([FromBody] ModuloCrearCommand command, CancellationToken ct)
    {
        var data = await sender.Send(command, ct);
        return Ok(data);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Modulo), 200)]
    [ProducesResponseType(typeof(ErrorMessage), 400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<Modulo>> Modificar(int id, [FromBody] ModuloModificarCommand command, CancellationToken ct)
    {
        var data = await sender.Send(command with { Id = id }, ct);
        return Ok(data);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ErrorMessage), 400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult> Eliminar(int id, CancellationToken ct)
    {
        await sender.Send(new ModuloEliminarCommand(id), ct);
        return NoContent();
    }
}
