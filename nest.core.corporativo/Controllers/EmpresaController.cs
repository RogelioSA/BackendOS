using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.corporativo.EmpresaServices;
using nest.core.dominio;
using nest.core.dominio.Corporativo.Empresa;

namespace nest.core.corporativo.Controllers
{
    /// <summary>
    /// Controlador para la gestión de empresas.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EmpresaController : ControllerBase
    {
        private readonly EmpresaService service;
        private readonly ILogger<EmpresaController> logger;

        public EmpresaController(EmpresaService service, ILogger<EmpresaController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene todas las empresas registradas.
        /// </summary>
        /// <returns>Lista de empresas.</returns>
        /// <response code="200">Empresas obtenidas exitosamente.</response>
        /// <response code="400">Error al obtener las empresas.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Empresa>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Empresa>>> ObtenerTodos()
        {
            try
            {
                var data = await service.ObtenerTodos();
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene una empresa por su identificador.
        /// </summary>
        /// <param name="id">ID de la empresa.</param>
        /// <returns>Una empresa.</returns>
        /// <response code="200">Empresa encontrada.</response>
        /// <response code="400">Error al obtener la empresa.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Empresa), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Empresa>> ObtenerPorId(int id)
        {
            try
            {
                var data = await service.ObtenerPorId(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todas las empresas activas.
        /// </summary>
        /// <returns>Lista de empresas activas.</returns>
        /// <response code="200">Empresas activas obtenidas exitosamente.</response>
        /// <response code="400">Error al obtener las empresas activas.</response>
        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<Empresa>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Empresa>>> ObtenerActivos()
        {
            try
            {
                var data = await service.ObtenerActivos();
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Crea una nueva empresa.
        /// </summary>
        /// <param name="registro">Datos de la empresa a registrar.</param>
        /// <returns>Empresa creada.</returns>
        /// <response code="200">Empresa creada exitosamente.</response>
        /// <response code="400">Error al crear la empresa.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Empresa), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Empresa>> Agregar([FromBody] EmpresaCrearDto registro)
        {
            try
            {
                var data = await service.Agregar(registro);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Actualiza los datos de una empresa.
        /// </summary>
        /// <param name="id">ID de la empresa a modificar.</param>
        /// <param name="registro">Datos actualizados de la empresa.</param>
        /// <returns>Empresa modificada.</returns>
        /// <response code="200">Empresa modificada exitosamente.</response>
        /// <response code="400">Error al modificar la empresa.</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(Empresa), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Empresa>> Modificar(int id, [FromBody] EmpresaCrearDto registro)
        {
            try
            {
                var data = await service.Modificar(id, registro);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Elimina una empresa.
        /// </summary>
        /// <param name="id">ID de la empresa a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="200">Empresa eliminada exitosamente.</response>
        /// <response code="400">Error al eliminar la empresa.</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(int id)
        {
            try
            {
                await service.Eliminar(id);
                return Ok(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }
    }
}

