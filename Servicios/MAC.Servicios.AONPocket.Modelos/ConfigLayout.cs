using System;
using System.Collections.Generic;
using System.Text;

namespace MAC.Servicios.AONPocket.Modelos
{
    public class ConfigLayout
    {
        public String RutaDescarga { get; set; }
        public Columna[] Columnas { get; set; }
        public class Columna
        {
            public String Destino { get; set; }
            public int Indice { get; set; }
        }
    }
}
