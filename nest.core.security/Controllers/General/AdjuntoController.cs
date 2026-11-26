using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.general.Adjuntos.Commands;
using nest.core.aplicacion.general.Adjuntos.Queries;
using nest.core.dominio;
using nest.core.dominio.General.AdjuntoEntities;
using nest.core.dominio.General.AdjuntoProviderEntities;
using nest.core.security.Extensions;
using nest.core.security.Models.General.Requests;

namespace nest.core.security.Controllers.General
{
    /// <summary>
    /// Gestiona los archivos adjuntos.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.GENERAL)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.GENERAL}/[controller]")]
    public class AdjuntoController : ControllerBase
    {
        private readonly ISender sender;

        public AdjuntoController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Obtiene todos los adjuntos registrados.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<Adjunto>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Adjunto>>> ObtenerTodos(CancellationToken cancellationToken)
        {
            var data = await sender.Send(new ObtenerTodosQuery(), cancellationToken);
            return Ok(data);
        }

        /// <summary>
        /// Obtiene un adjunto por su identificador.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Adjunto), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Adjunto>> ObtenerPorId(long id, CancellationToken cancellationToken)
        {
            var data = await sender.Send(new ObtenerPorIdQuery(id), cancellationToken);
            return Ok(data);
        }

        /// <summary>
        /// Registra un nuevo adjunto.
        /// </summary>
        [HttpPost("{modulo}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(Adjunto), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Adjunto>> Agregar([FromForm] AdjuntoUploadRequest request, [FromRoute]AdjuntoConfigProviderModuloEnum modulo, CancellationToken cancellationToken = default)
        {
            if (request.Archivo is null || request.Archivo.Length == 0)
                return BadRequest(new ErrorMessage { Message = "Debe adjuntar un archivo válido." });

            var stream = request.Archivo.OpenReadStream();
            var command = new AdjuntoCrearCommand(
                modulo,
                stream,
                request.Archivo.FileName,
                request.Archivo.ContentType,
                request.Archivo.Length);

            var data = await sender.Send(command, cancellationToken);
            return Ok(data);
        }

        /// <summary>
        /// Actualiza un adjunto existente.
        /// </summary>
        [HttpPut("{modulo}/{id}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(Adjunto), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Adjunto>> Modificar([FromRoute] long id, [FromForm] AdjuntoUploadRequest request, [FromRoute] AdjuntoConfigProviderModuloEnum modulo, CancellationToken cancellationToken = default)
        {
            if (request.Archivo is null || request.Archivo.Length == 0)
                return BadRequest(new ErrorMessage { Message = "Debe adjuntar un archivo válido." });

            var stream = request.Archivo.OpenReadStream();
            var command = new AdjuntoModificarCommand(
                id,
                modulo,
                stream,
                request.Archivo.FileName,
                request.Archivo.ContentType,
                request.Archivo.Length);

            var data = await sender.Send(command, cancellationToken);
            return Ok(data);
        }

        /// <summary>
        /// Elimina un adjunto.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(long id, CancellationToken cancellationToken = default)
        {
            await sender.Send(new AdjuntoEliminarCommand(id), cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Obtiene la url de un attachment para su descarga.
        /// </summary>
        [HttpGet("download/{id}")]
        [ProducesResponseType(typeof(Adjunto), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<string>> ObtenerUrl([FromRoute] long id, CancellationToken cancellationToken = default)
        {
            var data = await sender.Send(new ObtenerUrlDescargaQuery(id), cancellationToken);
            return Ok(data);
        }
    }
}
