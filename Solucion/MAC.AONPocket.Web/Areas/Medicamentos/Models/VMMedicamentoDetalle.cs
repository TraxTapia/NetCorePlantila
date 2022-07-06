using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace MAC.Models.Medicamentos
{
    public class VMMedicamentoDetalle
    {
        public int ID { get; set; }

        [Display(Description = "Nombre Genérico", Name = "Nombre Genérico")]
        public String NombreGenerico { get; set; }

        [Display(Description = "Concentración", Name = "Concentración")]
        public String Concentracion { get; set; }

        [Display(Description = "UDM", Name = "UDM")]
        public int UnidaddeMedidaId { get; set; }

        [Display(Description = "Presentación", Name = "Presentación")]
        public int PresentacionId { get; set; }

        [Display(Description = "Cantidad", Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Display(Description = "Unidad", Name = "Unidad")]
        public int UnidadId { get; set; }

        [Display(Description = "Grupo", Name = "Grupo")]
        public int GrupoId { get; set; }

        [Display(Description = "Clave", Name = "Clave")]
        public String Clave { get; set; }

        [Display(Description = "Sub Clave", Name = "Sub Clave")]
        public String SubClave { get; set; }

        [Display(Description = "Principal Indicación", Name = "Principal Indicación")]
        public String PrincipalIndicacion { get; set; }

        [Display(Description = "Otras Indicaciones", Name = "Otras Indicación")]
        public String OtrasIndicaciones { get; set; }

        [Display(Description = "Tipo de Actualización", Name = "Tipo de Actualización")]
        public String TipoActualizacion { get; set; }

        [Display(Description = "Número de Actualización", Name = "Número de Actualización")]
        public String NumeroActualizacion { get; set; }

        [Display(Description = "Uso Pediátrico", Name = "Uso Pediátrico")]
        public Boolean Pediatrico { get; set; }

        private Boolean _activo = true;
        public Boolean Activo { get=> _activo; set => _activo = value; }
    }
}
