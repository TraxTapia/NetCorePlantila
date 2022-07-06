using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MAC.Models.Medicamentos;
using MAC.ViewModel.Layout;
namespace MAC.AONPocket.Web.Areas.Medicamentos.Controllers
{
    public class MedicamentosController : Controller
    {
        [Area("Medicamentos")]
        public IActionResult Index()
        {
            VMTabla Modelo = new VMTabla();
            try
            {
                Modelo = Consultar();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
            }
            return View(Modelo);
        }

        private VMTabla Consultar()
        {
            List<VMMedicamento> Sales = new List<VMMedicamento>();
            String titulo = "Catálogo General de Sales";
            String url = String.Empty;
            String urlSelect = String.Empty;
            VMTabla Modelo = new VMTabla();
            VMMedicamento Sal = new VMMedicamento();
            try
            {
                Sales.Add(new VMMedicamento() { Cantidad=10, Concentracion="200", Grupo="ANALGESIA", ID=1, NombreGenerico="IBUPROFENO", Presentacion="TABLETA", Unidad="CAJA", UnidaddeMedida="MG", ViadeAdministracion="ORAL" });
                Sales.Add(new VMMedicamento() { Cantidad = 20, Concentracion = "500", Grupo = "ANALGESIA", ID = 2, NombreGenerico = "ACIDO ACETILSALICILICO", Presentacion = "TABLETA", Unidad = "CAJA", UnidaddeMedida = "MG", ViadeAdministracion = "ORAL" });
                VMTablaModel<VMMedicamento> tabla = new VMTablaModel<VMMedicamento>(Sal, url, 1, "ASC", "60vh", null, 1, urlSelect);
                tabla.addColumnOption("Detalle",String.Empty);
                tabla.addColumnOption("EANS Relacionados", String.Empty);
                tabla.addColumnOption("Especialidades",  String.Empty);
                tabla.Datos = Sales;
                tabla.NewRow = new VMMedicamento() { Cantidad = 0, Concentracion = "", Grupo = "", ID = 0, NombreGenerico = "", Presentacion = "", Unidad = "", UnidaddeMedida = "", ViadeAdministracion = "" };
                Modelo.titulo = titulo;
                tabla.cDefinicionJSON.orderByColumn = 0;
                tabla.cDefinicionJSON.orderByDirec = "asc";
                tabla.cDefinicionJSON.fnRowCallback = "registroCondiciones";
                tabla.cDefinicionJSON.jsonData = tabla.Datos.Cast<object>().ToList();
                tabla.cDefinicionJSON.newRowModel = tabla.NewRow;
                Modelo.cDefinicionJSON = tabla.cDefinicionJSON;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return Modelo;
        }
    }
}
