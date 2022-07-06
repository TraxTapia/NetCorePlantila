using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAC.Servicios.Modelos
{
    public class ExcelData
    {
        public String Hoja { get; set; }
        public String Columna { get; set; }
        public uint Fila { get; set; }
        public Object Valor { get; set; }
    }
}
