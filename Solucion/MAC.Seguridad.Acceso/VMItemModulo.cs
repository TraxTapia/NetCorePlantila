using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MAC.Serguridad.Acceso
{
    [DataContract]
    public class VMItemModulo
    {
        private int idItemModulo;
        private string descripcionItem;
        private string urlDestino;
        private string urlIcono;
        private int idItemPadre;
        private int idModulo;
        private bool activo;
        private int orden;
        private string version;

        [DataMember]
        public int IdItemModulo { get => idItemModulo; set => idItemModulo = value; }
        [DataMember]
        public string DescripcionItem { get => descripcionItem; set => descripcionItem = value; }
        [DataMember]
        public string UrlDestino { get => urlDestino; set => urlDestino = value; }
        [DataMember]
        public string UrlIcono { get => urlIcono; set => urlIcono = value; }
        [DataMember]
        public int IdItemPadre { get => idItemPadre; set => idItemPadre = value; }
        [DataMember]
        public int IdModulo { get => idModulo; set => idModulo = value; }
        [DataMember]
        public bool Activo { get => activo; set => activo = value; }
        [DataMember]
        public int Orden { get => orden; set => orden = value; }
        [DataMember]
        public string Version { get => version; set => version = value; }
    }
}
