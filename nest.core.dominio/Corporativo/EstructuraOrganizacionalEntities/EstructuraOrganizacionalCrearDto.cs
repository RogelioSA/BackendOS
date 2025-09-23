using System.ComponentModel.DataAnnotations;

namespace nest.core.dominio.Corporativo.EstructuraOrganizacionalEntities
{
    public class EstructuraOrganizacionalCrearDto
    {
        public int EmpresaId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        [StringLength(9)]
        public string NombreCorto { get; set; }
        public int? ParentId { get; set; }
        public int EstructuraOrganizacionalTipoId { get; set; }
        public bool Estado { get; set; }
        public bool Final { get; set; }
    }
}
