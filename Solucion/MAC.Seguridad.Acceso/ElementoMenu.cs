using System;
using System.Collections.Generic;
using System.Text;

namespace MAC.Serguridad.Acceso
{
    public class ElementoMenu
    {
        public String id { get; set; }

        public String father { get; set; }

        private String _texto = String.Empty;
        public String texto { get => _texto; set => _texto = value; }

        private String _descripcion = String.Empty;
        public String descripcion { get => _descripcion; set => _descripcion = value; }

        private String _imagen = "";
        public String imagen { get => _imagen; set => _imagen = value; }

        private String _tipo = "";
        public String tipo { get => _tipo; set => _tipo = value; }

        private String _accion = "";
        public String accion { get => _accion; set => _accion = value; }

        private String _controlador = "";
        public String controlador { get => _controlador; set => _controlador = value; }

        private String _parametros = "";
        public String parametros { get => _parametros; set => _parametros = value; }
    }
}
