using MAC.Servicios.Data.DAO.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MAC.Servicios.AONPocket.Entidades
{
    [Table("DOCUMENTACIONENVIO")]
    public class DocumentacionEnvio:Entidad
    {
        private String _OT = String.Empty;
        [Column("OT")]
        public string OT { get => _OT; set => _OT = value; }
        private String _archivo = String.Empty;
        [Column("archivo")]
        public string Archivo { get => _archivo; set => _archivo=value; }
        [Column("fecha_Envio")]
        public DateTime? FechaEnvio { get; set; }
    }
}
