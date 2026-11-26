using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.corporativo.Empresas.Commands;
using nest.core.aplicacion.corporativo.Empresas.Queries;
using nest.core.dominio;
using nest.core.dominio.Corporativo.Empresa;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Corporativo
{
    /// <summary>
    /// Controlador para la gestión de empresas.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.CORPORATIVO)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.CORPORATIVO}/[controller]")]
    public class EmpresaController : ControllerBase
    {
        private readonly ISender sender;

        public EmpresaController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Obtiene todas las empresas registradas.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<Empresa>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Empresa>>> ObtenerTodos(CancellationToken ct)
        {
            var entidades = await sender.Send(new ObtenerTodosQuery(), ct);
            return Ok(entidades);
        }

        /// <summary>
        /// Obtiene una empresa por su identificador.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Empresa), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Empresa?>> ObtenerPorId([FromRoute] int id, CancellationToken ct)
        {
            var entidad = await sender.Send(new ObtenerPorIdQuery(id), ct);
            return Ok(entidad);
        }

        /// <summary>
        /// Obtiene todas las empresas activas.
        /// </summary>
        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<Empresa>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Empresa>>> ObtenerActivos(CancellationToken ct)
        {
            var entidades = await sender.Send(new ObtenerActivosQuery(), ct);
            return Ok(entidades);
        }

        /// <summary>
        /// Obtiene todas las empresas aplicando filtros y paginación según las opciones de carga proporcionadas.
        /// </summary>
        [HttpPost("filter")]
        [ProducesResponseType(typeof(List<Empresa>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<LoadResult>> ObtenerFiltro([FromBody] DataSourceLoadOptionsBase loadOptions, CancellationToken ct)
        {
            var entidades = await sender.Send(new ObtenerFilterQuery(loadOptions), ct);
            return Ok(entidades);
        }

        /// <summary>
        /// Obtiene todas las empresas activas aplicando filtros y paginación según las opciones de carga proporcionadas.
        /// </summary>
        [HttpPost("filter_activos")]
        [ProducesResponseType(typeof(List<Empresa>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<LoadResult>> ObtenerFiltroActivos([FromBody] DataSourceLoadOptionsBase loadOptions, CancellationToken ct)
        {
            var entidades = await sender.Send(new ObtenerFilterActivosQuery(loadOptions), ct);
            return Ok(entidades);
        }

        /// <summary>
        /// Crea una nueva empresa.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Empresa), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Empresa>> Agregar([FromBody] EmpresaCrearCommand command, CancellationToken ct)
        {
            var entidad = await sender.Send(command, ct);
            return Ok(entidad);
        }

        /// <summary>
        /// Actualiza los datos de una empresa.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(Empresa), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Empresa>> Modificar([FromRoute] int id, [FromBody] EmpresaModificarCommand command, CancellationToken ct)
        {
            var entidad = await sender.Send(command with { Id = id }, ct);
            return Ok(entidad);
        }

        /// <summary>
        /// Elimina una empresa.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar([FromRoute] int id, CancellationToken ct)
        {
            await sender.Send(new EmpresaEliminarCommand(id), ct);
            return Ok(true);
        }
    }
}
