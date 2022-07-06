using MAC.Servicios.Data.DAO.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MAC.Servicios.AONPocket.Entidades
{
    [Table("DOCUMENTACIONENVIODETALLES")]
    public class DocumentacionEnvioDetalles:Entidad
    {
        public int envio { get; set; }
        public string iCodAfiliado { get; set; }
        public string nombre { get; set; }
        public string apellido_Paterno { get; set; }
        public string apellido_Materno { get; set; }
        public string parentesco { get; set; }
        public string poliza { get; set; }
        public string contrato { get; set; }
        public string certificado { get; set; }
        public String clave_Plan { get; set; }
        public string plan_dsc { get; set; }
        public string archivo_Original { get; set; }
        public string archivo_Final { get; set; }
        public String email { get; set; }
        public String email_Agente { get; set; }
        public String email_Promotor { get; set; }
        public String email_Ejecutivo { get; set; }
        public bool envioxeMail { get; set; }
        public bool envioxftp { get; set; }
        public String num_Solicitud { get; set; }
        public DateTime? fecha_Envio { get; set; }
    }
}
