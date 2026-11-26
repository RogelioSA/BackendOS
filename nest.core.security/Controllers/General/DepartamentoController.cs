using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.general.Departamentos.Commands;
using nest.core.aplicacion.general.Departamentos.Queries;
using nest.core.dominio;
using nest.core.dominio.General.DepartamentoEntites;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.General
{
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.GENERAL)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.GENERAL}/[controller]")]
    public class DepartamentoController : ControllerBase
    {
        private readonly ISender sender;

        public DepartamentoController(ISender sender)
        {
            this.sender = sender;
        }
        [HttpGet]
        [ProducesResponseType(typeof(List<Departamento>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Departamento>>> ObtenerTodos(CancellationToken cancellationToken)
        {
            var data = await sender.Send(new ObtenerTodosQuery(), cancellationToken);
            return Ok(data);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Departamento), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Departamento>> ObtenerPorId([FromRoute] int id, CancellationToken cancellationToken)
        {
            var data = await sender.Send(new ObtenerPorIdQuery(id), cancellationToken);
            return Ok(data);
        }
        [HttpPost]
        [ProducesResponseType(typeof(Departamento), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Departamento>> Agregar([FromBody] DepartamentoCrearCommand command, CancellationToken cancellationToken)
        {
            var data = await sender.Send(command, cancellationToken);
            return Ok(data);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Departamento), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Departamento>> Modificar([FromRoute] int id, [FromBody] DepartamentoModificarCommand command, CancellationToken cancellationToken)
        {
            var cmd = command with { Id = id };
            var data = await sender.Send(cmd, cancellationToken);
            return Ok(data);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar([FromRoute] int id, CancellationToken cancellationToken)
        {
            await sender.Send(new DepartamentoEliminarCommand(id), cancellationToken);
            return Ok(true);
        }
    }
}
