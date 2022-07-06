using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MAC.Serguridad.Acceso
{
    [DataContract]
    public class VMPerfil
    {
        VMRol rol;
        List<VMMapa<VMModuloApp, VMItemModulo>> listModulos;

        [DataMember]
        public VMRol Rol { get => rol; set => rol = value; }
        [DataMember]
        public List<VMMapa<VMModuloApp, VMItemModulo>> ListModulos { get => listModulos; set => listModulos = value; }
    }
}
