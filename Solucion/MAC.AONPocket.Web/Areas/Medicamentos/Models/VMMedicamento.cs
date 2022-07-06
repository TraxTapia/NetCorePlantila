using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MAC.Models.Medicamentos
{
    public class VMMedicamento
    {
        public int ID { get; set; }

        [Display(Description = "Nombre Genérico", Name = "Nombre Genérico")]
        public String NombreGenerico { get; set; }

        [Display(Description = "Concentración", Name = "Concentración")]
        public String Concentracion { get; set; }

        [Display(Description = "UDM", Name = "UDM")]
        public String UnidaddeMedida { get; set; }

        [Display(Description = "Presentación", Name = "Presentación")]
        public String Presentacion { get; set; }

        [Display(Description = "Vía de Administración", Name = "Vía de Administración")]
        public String ViadeAdministracion { get; set; }

        [Display(Description = "Cantidad", Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Display(Description = "Unidad", Name = "Unidad")]
        public String Unidad { get; set; }

        [Display(Description = "Grupo", Name = "Grupo")]
        public String Grupo { get; set; }
    }
}
