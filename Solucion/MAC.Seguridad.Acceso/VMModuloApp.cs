using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MAC.Serguridad.Acceso
{
    [DataContract]
    public class VMModuloApp
    {
        private int idModulo;
        private string nombreModulo;
        private string urlIcono;
        private string urlDestino;
        private int idApp;
        private bool activo;
        private string version;

        [DataMember]
        public int IdModulo { get => idModulo; set => idModulo = value; }
        [DataMember]
        public string NombreModulo { get => nombreModulo; set => nombreModulo = value; }
        [DataMember]
        public string UrlIcono { get => urlIcono; set => urlIcono = value; }
        [DataMember]
        public string UrlDestino { get => urlDestino; set => urlDestino = value; }
        [DataMember]
        public int IdApp { get => idApp; set => idApp = value; }
        [DataMember]
        public bool Activo { get => activo; set => activo = value; }
        [DataMember]
        public string Version { get => version; set => version = value; }
    }

}
