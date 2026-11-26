using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.mantto.OrdenTrabajoHorarios.Commands;
using nest.core.aplicacion.mantto.OrdenTrabajoHorarios.Queries;
using nest.core.dominio;
using nest.core.dominio.Mantto.OrdenTrabajoHorarioEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Mantto
{
    /// <summary>
    /// Controlador para la gestión de horarios de órdenes de trabajo.
    /// Permite realizar operaciones CRUD básicas.
    /// Requiere autorización para acceder.
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.MANTTO)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.MANTTO}/[controller]")]
    [ApiController]
    public class OrdenTrabajoHorarioController : ControllerBase
    {
        private readonly ISender sender;
        private readonly ILogger<OrdenTrabajoHorarioController> logger;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="OrdenTrabajoHorarioController"/>.
        /// </summary>
        /// <param name="sender">Mediador para ejecutar los comandos y consultas.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        public OrdenTrabajoHorarioController(ISender sender, ILogger<OrdenTrabajoHorarioController> logger)
        {
            this.sender = sender;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene todos los horarios de órdenes de trabajo registrados.
        /// </summary>
        /// <returns>Lista de horarios.</returns>
        /// <response code="200">Devuelve la lista de horarios.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<OrdenTrabajoHorario>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<OrdenTrabajoHorario>>> ObtenerTodos()
        {
            var data = await sender.Send(new ObtenerTodosQuery());
            return Ok(data);
        }

        /// <summary>
        /// Obtiene un horario de orden de trabajo por su identificador.
        /// </summary>
        /// <param name="request">Parametros de búsqueda.</param>
        /// <returns>Horario correspondiente al identificador indicado.</returns>
        /// <response code="200">Horario encontrado.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("by_ot_and_date_range")]
        [ProducesResponseType(typeof(OrdenTrabajoHorario), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<OrdenTrabajoHorario>>> ObtenerPorOtYRangoFechas([FromQuery] ObtenerPorOtYRangoFechasQuery request)
        {
            var data = await sender.Send(request);
            return Ok(data);
        }

        /// <summary>
        /// Obtiene un horario de orden de trabajo por su identificador.
        /// </summary>
        /// <param name="id">Identificador del horario.</param>
        /// <returns>Horario correspondiente al identificador indicado.</returns>
        /// <response code="200">Horario encontrado.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrdenTrabajoHorario), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenTrabajoHorario>> ObtenerPorId(long id)
        {
            var data = await sender.Send(new ObtenerPorIdQuery(id));
            return Ok(data);
        }

        /// <summary>
        /// Registra un nuevo horario de orden de trabajo.
        /// </summary>
        /// <param name="command">Comando con la información del horario a crear.</param>
        /// <returns>Horario creado.</returns>
        /// <response code="200">Horario creado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost]
        [ProducesResponseType(typeof(OrdenTrabajoHorario), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenTrabajoHorario>> Agregar([FromBody] OrdenTrabajoHorarioCrearCommand command)
        {
            var data = await sender.Send(command);
            return Ok(data);
        }

        /// <summary>
        /// Registra un rango horario de orden de trabajo.
        /// </summary>
        /// <param name="command">Comando con la información del horario a crear.</param>
        /// <returns>Horarios creados.</returns>
        /// <response code="200">Horarios creados exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost("range")]
        [ProducesResponseType(typeof(OrdenTrabajoHorario[]), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenTrabajoHorario[]>> AgregarRango([FromBody] OrdenTrabajoHorarioCrearRangoCommand command)
        {
            var data = await sender.Send(command);
            return Ok(data);
        }

        /// <summary>
        /// Modifica un horario de orden de trabajo existente.
        /// </summary>
        /// <param name="id">Identificador del horario a modificar.</param>
        /// <param name="command">Comando con la información actualizada del horario.</param>
        /// <returns>Horario modificado.</returns>
        /// <response code="200">Horario modificado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OrdenTrabajoHorario), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenTrabajoHorario>> Modificar(long id, [FromBody] OrdenTrabajoHorarioModificarCommand command)
        {
            var data = await sender.Send(command with { Id = id });
            return Ok(data);
        }

        /// <summary>
        /// Elimina un horario de orden de trabajo.
        /// </summary>
        /// <param name="id">Identificador del horario a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="200">Horario eliminado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(long id)
        {
            await sender.Send(new OrdenTrabajoHorarioEliminarCommand(id));
            return Ok(true);
        }
    }
}
