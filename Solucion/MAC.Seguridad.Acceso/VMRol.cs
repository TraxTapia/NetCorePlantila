using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MAC.Serguridad.Acceso
{
    [DataContract]
    public class VMRol
    {
        private int idRol;
        private string nombreRol;
        private string descripcion;
        private bool activo;
        private int idApp;

        [DataMember]
        public int IdRol { get => idRol; set => idRol = value; }
        [DataMember]
        public string NombreRol { get => nombreRol; set => nombreRol = value; }
        [DataMember]
        public string Descripcion { get => descripcion; set => descripcion = value; }
        [DataMember]
        public bool Activo { get => activo; set => activo = value; }
        [DataMember]
        public int IdApp { get => idApp; set => idApp = value; }

    }
}
