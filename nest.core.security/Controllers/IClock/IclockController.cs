using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.iclock.Marcaciones.Commands;
using nest.core.dominio;
using nest.core.security.Extensions;
using nest.core.security.Models.IClock;
using System.Globalization;

namespace nest.core.security.Controllers.IClock
{
    /// <summary>
    /// Controlador compatible con terminales biométricos que publican marcaciones mediante el protocolo iClock.
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.ICLOCK)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.ICLOCK}")]
    public class IclockController : ControllerBase
    {
        private readonly ISender sender;
        private readonly ILogger<IclockController> logger;
        private readonly int empresaId;

        public IclockController(ISender sender, ILogger<IclockController> logger, IConfiguration configuration)
        {
            this.sender = sender;
            this.logger = logger;
            empresaId = configuration.GetValue<int?>("Iclock:EmpresaId") ?? 1;
        }

        /// <summary>
        /// Endpoint de alta/keep-alive del dispositivo iClock.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public ActionResult<string> Get()
        {
            logger.LogInformation("Get iClock recibido");
            return Content("OK", "text/plain");
        }

        /// <summary>
        /// Endpoint de alta/keep-alive del dispositivo iClock.
        /// </summary>
        [HttpGet("cdata")]
        [ProducesResponseType(typeof(string), 200)]
        public ActionResult<string> Handshake([FromQuery] string SN)
        {
            logger.LogInformation("Handshake iClock recibido desde {SerialNumber}", SN);
            return Content("OK", "text/plain");
        }

        /// <summary>
        /// Recibe una o varias marcaciones de asistencia enviadas por el reloj biométrico.
        /// </summary>
        [HttpPost("cdata")]
        [Consumes("text/plain", "application/octet-stream", "application/x-www-form-urlencoded")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<string>> RecibirMarcaciones([FromQuery] string SN, CancellationToken ct)
        {
            using var reader = new StreamReader(Request.Body);
            var payload = await reader.ReadToEndAsync(ct);

            var marcas = payload.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in marcas)
            {
                try
                {
                    string[] paramsMarca = line.Split("\t");
                    logger.LogWarning($"El registro recibido SN: {SN} | Payload: {line} | Length: {paramsMarca.Length}");
                    if (!string.IsNullOrWhiteSpace(line) && paramsMarca.Length >= 2)
                    {
                        string dni = line.Split("\t")[0];
                        string fechaStr = line.Split("\t")[1];
                        RecibirMarcacionesCommand command = new RecibirMarcacionesCommand(1, dni, SN, DateTime.Parse(fechaStr));
                        await sender.Send(command, ct);
                    }
                    else
                        logger.LogError($"El registro recibido no cumple el formato de marca SN: {SN} | Payload: {line}");
                }
                catch (Exception ex)
                {
                    logger.LogError("Error: " + ex.Message);
                }
            }
            return Content("OK", "text/plain");
        }

        /// <summary>
        /// Permite validar manualmente el procesamiento de marcaciones desde clientes REST.
        /// </summary>
        [HttpPost("attendance")]
        [ProducesResponseType(typeof(IclockResponse), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<IclockResponse>> RecibirMarcacionesRest([FromBody] string payload, [FromQuery] string SN, CancellationToken ct)
        {
            var resultado = await ProcesarMarcaciones(payload, SN, ct);
            return Ok(resultado);
        }

        /// <summary>
        /// Endpoint consultado por dispositivos iClock para obtener comandos pendientes.
        /// </summary>
        [HttpGet("getrequest")]
        [ProducesResponseType(typeof(string), 200)]
        public ActionResult<string> ObtenerComandos([FromQuery] string SN)
        {
            logger.LogDebug("Consulta de comandos iClock desde {SerialNumber}", SN);
            return Content("OK", "text/plain");
        }

        /// <summary>
        /// Confirma la recepción de respuestas a comandos ejecutados por el dispositivo.
        /// </summary>
        [HttpPost("devicecmd")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<ActionResult<string>> ConfirmarComando([FromQuery] string SN, CancellationToken ct)
        {
            using var reader = new StreamReader(Request.Body);
            var payload = await reader.ReadToEndAsync(ct);
            logger.LogInformation("Respuesta de comando iClock recibida desde {SerialNumber}: {Payload}", SN, payload);
            return Content("OK", "text/plain");
        }

        private async Task<IclockResponse> ProcesarMarcaciones(string payload, string serialNumber, CancellationToken ct)
        {
            var procesados = 0;
            var omitidos = 0;
            var errores = new List<string>();

            foreach (var line in ObtenerLineas(payload))
            {
                if (!TryParseMarcacion(line, serialNumber, out var record, out var error))
                {
                    omitidos++;
                    errores.Add(error);
                    continue;
                }

                if (!int.TryParse(record.Pin, out var personalId))
                {
                    omitidos++;
                    errores.Add($"El PIN '{record.Pin}' no corresponde a un PersonalId numérico.");
                    continue;
                }

                //await sender.Send(new RegistroAsistenciaCrearCommand(empresaId, personalId, record.Fecha, null, null), ct);
                procesados++;
            }

            return new IclockResponse(serialNumber, procesados, omitidos, errores);
        }

        private static IEnumerable<string> ObtenerLineas(string payload)
        {
            return (payload ?? string.Empty)
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x));
        }

        private static bool TryParseMarcacion(string line, string serialNumber, out IclockAttendanceRecord record, out string error)
        {
            record = null;
            error = null;

            var parts = line.Split(new[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                error = $"La línea '{line}' no tiene el formato esperado.";
                return false;
            }

            var fechaTexto = parts.Length > 2 ? $"{parts[1]} {parts[2]}" : parts[1];
            if (!DateTime.TryParseExact(fechaTexto, new[] { "yyyy-MM-dd HH:mm:ss", "yyyy/MM/dd HH:mm:ss", "yyyy-MM-ddTHH:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var fecha))
            {
                error = $"La fecha '{fechaTexto}' de la línea '{line}' no es válida.";
                return false;
            }

            var offset = parts.Length > 2 ? 3 : 2;
            record = new IclockAttendanceRecord(
                parts[0],
                fecha,
                parts.ElementAtOrDefault(offset) ?? string.Empty,
                parts.ElementAtOrDefault(offset + 1) ?? string.Empty,
                parts.ElementAtOrDefault(offset + 2) ?? string.Empty,
                parts.ElementAtOrDefault(offset + 3) ?? string.Empty,
                serialNumber);
            return true;
        }
    }
}
