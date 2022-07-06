using System;
using System.Collections.Generic;
using System.Text;

namespace MAC.Serguridad.Acceso
{
    public class Menu
    {

        private contenido _Contenido = new contenido();
        public contenido MenuContent { get => _Contenido; set => _Contenido = value; }

        public class contenido
        {
            private List<ElementoMenu> _Opciones = new List<ElementoMenu>();
            public List<ElementoMenu> Opciones { get => _Opciones; set => _Opciones = value; }
        }
    }
}
