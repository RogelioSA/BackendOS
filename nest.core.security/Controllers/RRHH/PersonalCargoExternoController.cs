using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.rrhh.PersonalCargoExternos.Commands;
using nest.core.aplicacion.rrhh.PersonalCargoExternos.Queries;
using nest.core.dominio;
using nest.core.dominio.RRHH.PersonalCargoExternoEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.RRHH
{
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.RRHH)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.RRHH}/[controller]")]
    [ApiController]
    public class PersonalCargoExternoController : ControllerBase
    {
        private readonly ISender sender;

        public PersonalCargoExternoController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PersonalCargoExterno>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<PersonalCargoExterno>>> ObtenerTodos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPersonalCargoExternosQuery(), ct);
            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PersonalCargoExterno), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<PersonalCargoExterno>> ObtenerPorId(long id, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPersonalCargoExternoPorIdQuery(id), ct);
            return Ok(data);
        }

        [HttpGet("personal/{personalId}")]
        [ProducesResponseType(typeof(List<PersonalCargoExterno>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<PersonalCargoExterno>>> ObtenerPorPersonal(int personalId, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPersonalCargoExternosPorPersonalQuery(personalId), ct);
            return Ok(data);
        }

        [HttpGet("cargo/{cargoId}")]
        [ProducesResponseType(typeof(List<PersonalCargoExterno>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<PersonalCargoExterno>>> ObtenerPorCargo(int cargoId, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPersonalCargoExternosPorCargoQuery(cargoId), ct);
            return Ok(data);
        }

        [HttpPost("filter")]
        [ProducesResponseType(typeof(LoadResult), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoadResult>> ObtenerPorFiltro([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPersonalCargoExternosPorFiltroDataSourceQuery(options), ct);
            return Ok(data);
        }

        [HttpPost("filter_activos")]
        [ProducesResponseType(typeof(LoadResult), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoadResult>> ObtenerPorFiltroActivos([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPersonalCargoExternosPorFiltroActivosQuery(options), ct);
            return Ok(data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PersonalCargoExterno), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<PersonalCargoExterno>> Agregar([FromBody] PersonalCargoExternoCrearCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PersonalCargoExterno), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<PersonalCargoExterno>> Modificar(long id, [FromBody] PersonalCargoExternoModificarCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command with { Id = id }, ct);
            return Ok(data);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(long id, CancellationToken ct)
        {
            await sender.Send(new PersonalCargoExternoEliminarCommand(id), ct);
            return Ok();
        }
    }
}
