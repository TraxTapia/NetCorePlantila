using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MAC.Models.Medicamentos
{
    public class VMSAL
    {
        [Display(Description = "Sal", Name = "Sal")]
        public int SALId { get; set; }
        
        [Display(Description = "Concentración", Name = "Concentración")]
        public String Concentracion { get; set; }

        [Display(Description = "UDM", Name = "UDM")]
        public int UnidaddeMedidaId { get; set; }
        public String  Etiqueta { get; set; }
    }
}
