using nest.core.dominio.RRHH.HorarioDetalleEventoEntities;
using nest.core.dominio.RRHH.PersonalCargoExternoEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaAdjuntosEntities;
namespace nest.core.dominio.RRHH.RegistroAsistenciaEntities
{
    public class RegistroAsistenciaQueryView
    {
        public int? EmpresaId { get; set; }
        public long? Id { get; set; }
        public int? PersonalId { get; set; }
        public DateTime? Fecha { get; set; }
        public DateOnly? FechaJornal { get; set; }
        public HorarioDetalleEventoTipoEnum? TipoEvento { get; set; }
        public bool? EsTardanza { get; set; }
        public int? DiferenciaMinutos { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public long? AdjuntoId { get; set; }
        public string AdjuntoUrl { get; set; }
        public int? MinutosDescanso { get; set; }
        public int? MinutosTraslado { get; set; }
        public PersonalQueryView? Personal { get; set; }
        public PersonaQueryView? Persona { get; set; }
        public OrdenTrabajoQueryView? OrdenTrabajo { get; set; }
        public OrdenServicioQueryView? OrdenServicio { get; set; }
        public PersonalCargoExterno PersonalCargoExterno { get; set; }
        public List<AdjuntosQueryView> Adjuntos { get; set; }

    }

    public class PersonalQueryView
    {
        //public int? Id { get; set; }
        //public bool? MarcaAsistencia { get; set; }
        //public long? ContratoCabeceraId { get; set; }
        public int? HorarioCabeceraId { get; set; }
        //public long? RegistroAsistenciaPoliticaId { get; set; }

    }

    public class PersonaQueryView
    {
        //public int? Id { get; set; }
        //public string Nombres { get; set; }
        //public string ApellidoPaterno { get; set; }
        //public string ApellidoMaterno { get; set; }
        public string NombreCompleto { get; set; }
        //public DateTime? FechaNacimiento { get; set; }
        public string DocumentoIdentidad { get; set; }
        //public string Correo { get; set; }
        //public string Celular { get; set; }
        //public string Direccion { get; set; }

    }
    public class OrdenTrabajoQueryView
    {
        public long? Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        //public DateTime? FechaInicio { get; set; }
        //public DateTime? FechaCompromiso { get; set; }
        //public DateTime? FechaFin { get; set; }

    }

    public class OrdenServicioQueryView
    {
        public long? Id { get; set; }
        //public short? OrdenServicioTipoId { get; set; }
        public string CodigoOrdenInterna { get; set; }
        public string CodigoReferencial { get; set; }
        public string Descripcion { get; set; }
        //public DateTime? FechaInicial { get; set; }
        //public DateTime? FechaFinal { get; set; }

    }

    public class AdjuntosQueryView
    {
        public long? Id { get; set; }
        public long? AdjuntoId { get; set; }
        public RegistroAsistenciaAdjuntoTipoEnum? Tipo { get; set; }
        public string? AdjuntoUrl { get; set; }

    }
}
