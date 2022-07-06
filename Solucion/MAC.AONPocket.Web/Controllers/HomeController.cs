using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MAC.AONPocket.Web.App_Code;
using UniversalModeloNegocio.RespuestaServ;
using UniversalModeloNegocio.Mensajeria;
using MACServiceGenerico.Respuesta;
using MACServiceGenerico.Peticion;
using Microsoft.AspNetCore.Http;
using MAC.AONPocket.Web.Models.APP;
using MAC.AONPocket.Web.Models;
using MAC.Servicios.ClienteRestNC;
using MAC.Serguridad.Acceso;
using System.IO;
using MAC.Servicios.Modelos;
using MAC.AONPocket.Models;
using UniversalModeloNegocio.AONPocket;
using MAC.Servicios.Data.DAO.EF;
using MAC.Servicios.AONPocket.Entidades;
namespace MAC.AONPocket.Web.Controllers
{
	[Autenticacion]
	public class HomeController : Controller
    {
		private String apiUrlMensajeria = ReadConfig.ReadKey("AppServices", "ServiciosMensajeria");
		private String apiUrl = ReadConfig.ReadKey("AppServices", "ServiciosAvisosExpediente");
		private String apiUrlLog = ReadConfig.ReadKey("LogService", "resturl");
		private String apiUrlAON = ReadConfig.ReadKey("AppServices", "ServiciosAONPocket");
		private int IdCarga = 1;
		public IActionResult Index()
        {
			TablaGenerica data = new TablaGenerica();
			try
			{
				
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message.ToString();
			}
			return View("Index",data);
		}

		private async Task ConsultarPlanes()
        {
			try
            {
				ViewBag.PlanesDocumentacion = await ApiPlanes();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private async Task<List<PlanCondicionesGenerales>> ApiPlanes()
		{
			List<PlanCondicionesGenerales> PlanesDocumentacion = new List<PlanCondicionesGenerales>();
			try
			{
				ClasePeticion<String> peticion = new ClasePeticion<String>();
				ClienteRest<RespuestaData<List<PlanCondicionesGenerales>>> cliente = new ClienteRest<RespuestaData<List<PlanCondicionesGenerales>>>();
				peticion.Clase = String.Empty;
				peticion.Token = GetToken();
				RespuestaData<List<PlanCondicionesGenerales>> respuesta = await cliente.LLamarServicioPostGeneral<ClasePeticion<String>>(apiUrlAON, ReadConfig.ReadKey("AppServices", "ConsultarPlanes"), peticion);
				if (respuesta.Respuesta.result == 1)
				{
					PlanesDocumentacion = respuesta.Datos;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return PlanesDocumentacion;
		}

		public async Task<IActionResult> Transferencia()
		{
			try
			{
				await ConsultarPlanes();
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message.ToString();
			}
			return View();
		}

		public async Task<IActionResult> Historico()
		{
			List<VMDocumentacionEnvio> Envios = new List<VMDocumentacionEnvio>();
			try
			{
				DateTime fechaFin = System.DateTime.Today;
				DateTime fechaInicio = fechaFin.AddMonths(-3);
				ViewBag.fechaInicio = fechaInicio;
				ViewBag.fechaFin = fechaFin;
				FiltroFechas filtro = new FiltroFechas();
				filtro.fechaInicial = (DateTime?)fechaInicio;
				filtro.fechaFinal = (DateTime?)fechaFin;
				Envios = await ListaEnviosFechas(filtro, GetToken());
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message.ToString();
			}
			return View("Historico",Envios);
		}

		[HttpPost]
		public async Task<IActionResult> FiltroFechas(DateTime fechaInicio, DateTime fechaFin)
		{
			List<VMDocumentacionEnvio> Envios = new List<VMDocumentacionEnvio>();
			try
			{
				ViewBag.fechaInicio = fechaInicio;
				ViewBag.fechaFin = fechaFin;
				FiltroFechas filtro = new FiltroFechas();
				filtro.fechaInicial = (DateTime?)fechaInicio;
				filtro.fechaFinal = (DateTime?)fechaFin;
				Envios = await ListaEnviosFechas(filtro, GetToken());
				await ConsultarPlanes();
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message.ToString();
			}
			return View("Historico", Envios);
		}

		public async Task<IActionResult> PendientesEnvio()
		{
			List<VMDocumentacionEnvio> Envios = new List<VMDocumentacionEnvio>();
			try
			{
				DateTime fechaFin = System.DateTime.Today;
				DateTime fechaInicio = fechaFin.AddMonths(-3);
				Envios = await ListaEnvios(null,GetToken());
				ViewBag.fechaInicio = fechaInicio;
				ViewBag.fechaFin = fechaFin;
				ViewBag.Funcion = "SelectPendiente";
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message.ToString();
			}
			return PartialView("_HistoricoPartial", Envios);
		}

		[HttpPost]
		public async Task<IActionResult> DetalleEnvio(int Id)
		{
			List<VMDocumentacionEnvioDetalles> Detalle = new List<VMDocumentacionEnvioDetalles>();
			try
			{
				Detalle = await DetalleEnvio(Id, GetToken());
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message.ToString();
			}
			return PartialView("_HistoricoDetalle", Detalle);
		}

		[HttpPost]
		public async Task<IActionResult> Envio(int Id)
		{
			TablaGenerica data = new TablaGenerica();
			try
			{
				data = await leerLayout(Id);
				await ConsultarPlanes();
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message.ToString();
			}
			return View("Transferencia", data);
		}

		private async Task<List<VMDocumentacionEnvio>> ListaEnvios(DateTime? fecha,String Token)
		{
			List<VMDocumentacionEnvio> Envios = new List<VMDocumentacionEnvio>();
			try
			{
				ClasePeticion<DateTime?> peticion = new ClasePeticion<DateTime?>();
				ClienteRest<RespuestaData<List<VMDocumentacionEnvio>>> cliente = new ClienteRest<RespuestaData<List<VMDocumentacionEnvio>>>();
				peticion.Clase = fecha;
				peticion.Token = Token;
				RespuestaData<List<VMDocumentacionEnvio>> respuesta = await cliente.LLamarServicioPostGeneral<ClasePeticion<DateTime?>>(apiUrlAON, ReadConfig.ReadKey("AppServices", "ConsultarFechaEnvio"), peticion);
				if (respuesta.Respuesta.result == 1)
				{
					Envios = respuesta.Datos;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return Envios;
		}

		private async Task<List<VMDocumentacionEnvio>> ListaEnviosFechas(FiltroFechas filtro, String Token)
		{
			List<VMDocumentacionEnvio> Envios = new List<VMDocumentacionEnvio>();
			try
			{
				ClasePeticion<FiltroFechas> peticion = new ClasePeticion<FiltroFechas>();
				ClienteRest<RespuestaData<List<VMDocumentacionEnvio>>> cliente = new ClienteRest<RespuestaData<List<VMDocumentacionEnvio>>>();
				peticion.Clase = filtro;
				peticion.Token = Token;
				RespuestaData<List<VMDocumentacionEnvio>> respuesta = await cliente.LLamarServicioPostGeneral<ClasePeticion<FiltroFechas>>(apiUrlAON, ReadConfig.ReadKey("AppServices", "ConsultarFechas"), peticion);
				if (respuesta.Respuesta.result == 1)
				{
					Envios = respuesta.Datos;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return Envios;
		}

		private async Task<List<VMDocumentacionEnvioDetalles>> DetalleEnvio(int id, String Token)
		{
			List<VMDocumentacionEnvioDetalles> Detalle = new List<VMDocumentacionEnvioDetalles>();
			try
			{
				ClasePeticion<int> peticion = new ClasePeticion<int>();
				ClienteRest<RespuestaData<List<VMDocumentacionEnvioDetalles>>> cliente = new ClienteRest<RespuestaData<List<VMDocumentacionEnvioDetalles>>>();
				peticion.Clase = id;
				peticion.Token = Token;
				RespuestaData<List<VMDocumentacionEnvioDetalles>> respuesta = await cliente.LLamarServicioPostGeneral<ClasePeticion<int>>(apiUrlAON, ReadConfig.ReadKey("AppServices", "DetalleEnvio"), peticion);
				if (respuesta.Respuesta.result == 1)
				{
					Detalle = respuesta.Datos;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return Detalle;
		}


		private void copyFiles(IFormFile[] files, String ruta, String timeAdd="")
		{
			try
			{
				if (!Directory.Exists(ruta))
				{
					Directory.CreateDirectory(ruta);
				}
				foreach (var file in files)
				{
					// Extract file name from whatever was posted by browser
					var fileName = Path.GetFileName(file.FileName);

					// Create new local file and copy contents of uploaded file
					using (var localFile = System.IO.File.OpenWrite(ruta + timeAdd + fileName))
					using (var uploadedFile = file.OpenReadStream())
					{
						uploadedFile.CopyTo(localFile);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private String GetToken()
		{
			String token = String.Empty;
			if (!String.IsNullOrEmpty(HttpContext.Session.GetString("AppToken")))
			{
				token = HttpContext.Session.GetString("AppToken");
			}
			return token;
		}

		[HttpPost]
		public async Task<IActionResult> UpLoad(IFormFile[] files,String OT)
		{
			TablaGenerica data = new TablaGenerica();
			try
			{
				String time =  System.DateTime.Now.ToString("yyyyMMddHHmmss");
				String ruta = ReadConfig.ReadKey("Layout", "RutaDescarga");
				ClasePeticion<VMDocumentacionEnvio> peticion = new ClasePeticion<VMDocumentacionEnvio>();
				VMDocumentacionEnvio envio=new VMDocumentacionEnvio();
				envio.UsuCambio = String.Empty;
				envio.Id = 0;
				envio.FechaCambio = null;
				envio.FechaEnvio = null;
				envio.OT = OT;
				String fileOT = files[0].FileName;
				envio.Archivo = String.Concat(time, fileOT);
				peticion.Token = GetToken();
				peticion.Clase = envio; 
				ClienteRest<RespuestaData<VMDocumentacionEnvio>> cliente = new ClienteRest<RespuestaData<VMDocumentacionEnvio>>();
				RespuestaData<VMDocumentacionEnvio> respuesta = await cliente.LLamarServicioPostGeneral<ClasePeticion<VMDocumentacionEnvio>>(apiUrlAON, ReadConfig.ReadKey("AppServices", "RegistrarEnvio"), peticion);
				if (respuesta.Respuesta.result == 1)
				{
					IdCarga = respuesta.Datos.Id;
					HttpContext.Session.SetString("IdCarga", IdCarga.ToString());
					HttpContext.Session.SetString("IdOT", OT);
					ViewBag.OT = OT;
					envio.Id = IdCarga;
					peticion.Clase = envio;
					ruta = String.Concat(ruta, IdCarga.ToString(), "\\");
					copyFiles(files, ruta, time);
					ClienteRest<RespuestaData<List<String>>> clienteDetalle = new ClienteRest<RespuestaData<List<String>>>();
					RespuestaData<List<String>> respuestaDetalle = await clienteDetalle.LLamarServicioPostGeneral<ClasePeticion<VMDocumentacionEnvio>>(apiUrlAON, ReadConfig.ReadKey("AppServices", "RegistrarDetalleEnvio"), peticion);
					if (respuestaDetalle.Respuesta.result == 1)
					{
						data = await leerLayout(IdCarga);
					}
					else
					{
						ViewBag.Error= string.Join(",", respuestaDetalle.Datos);
					}
				}
				else
				{
					ViewBag.Error = respuesta.Respuesta.mensaje;
				}
				await ConsultarPlanes();
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message.ToString();
			}
			return View("Transferencia",data);
		}

		[HttpPost]
		public async Task<IActionResult> SeleccionarEnvio(int Id,String IdOT)
        {
			TablaGenerica data = new TablaGenerica();
            try
            {
				ViewBag.Id = Id;
				data = await leerLayout(Id);
				ViewBag.OT = IdOT;
				ViewBag.DocCargada = true;
				HttpContext.Session.SetString("IdCarga", Id.ToString());
				HttpContext.Session.SetString("IdOT", IdOT);
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message.ToString();
			}
			return PartialView("_DetalleOTPartial", data);
		}

		[HttpPost]
		public async Task<IActionResult> UpLoadDocumentos(IFormFile[] doctos)
		{
			TablaGenerica data = new TablaGenerica();
			try
			{
				String ruta = ReadConfig.ReadKey("Layout", "RutaDescarga");
				IdCarga=Convert.ToInt32(HttpContext.Session.GetString("IdCarga"));
				ViewBag.OT = HttpContext.Session.GetString("IdOT");
				ruta = String.Concat(ruta, IdCarga.ToString(), "\\");
				copyFiles(doctos, ruta);
				data = await leerLayout(IdCarga);
				ViewBag.DocCargada = true;
				await ConsultarPlanes();
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message.ToString();
			}
			return View("Transferencia", data);
		}

		[HttpPost]
		public JsonResult UpLoadCondiciones(IFormFile[] condiciones,String plandoc)
		{
			TablaGenerica data = new TablaGenerica();
			try
			{
				String ruta = ReadConfig.ReadKey("Layout", "RutaDescarga");
				ruta = String.Concat(ruta, plandoc, "\\");
				if (Directory.Exists(ruta))
				{
					List<string> strFiles = Directory.GetFiles(ruta, "*", SearchOption.AllDirectories).ToList();

					foreach (string fichero in strFiles)
					{
						System.IO.File.Delete(fichero);
					}
                }
                else
                {
					Directory.CreateDirectory(ruta);
                }
				copyFiles(condiciones, ruta);
			}
			catch (Exception ex)
			{
				return Json(new { mensaje = ex.Message.ToString(), success = 0 });
			}
			return Json(new { mensaje = String.Empty, success = 1 });
		}

		[HttpPost]
		public async Task<IActionResult> EnviarDocumentacion(Envio envio)
		{
			TablaGenerica data = new TablaGenerica();
			try
			{
				String accion = envio.tipoEnvio == 2? ReadConfig.ReadKey("AppServices", "EnviarCorreos") : ReadConfig.ReadKey("AppServices", "EnviarAON");
				ClasePeticion<Envio> peticion = new ClasePeticion<Envio>();
				String cveCarga = HttpContext.Session.GetString("IdCarga");
				envio.Id = int.Parse(cveCarga);
				peticion.Clase = envio;
				ClienteRest<RespuestaData<List<String>>> cliente = new ClienteRest<RespuestaData<List<String>>>();
				RespuestaData <List<String>> respuesta = await cliente.LLamarServicioPostGeneral(apiUrlAON, accion, peticion);
				ViewBag.EnvioOk = false;
				if (respuesta!=null && respuesta.Datos != null)
                {
					ViewBag.Avisos = respuesta.Datos;
					ViewBag.EnvioOk = respuesta.Datos.Count > 0 ? false : true;
                }
                else
                {
					List<String> errordeEnvio = new List<String>();
					errordeEnvio.Add(String.IsNullOrEmpty(respuesta.Respuesta.mensaje) ? "Error de envío" : respuesta.Respuesta.mensaje);
					ViewBag.Avisos= errordeEnvio;
				}
				data = await leerLayout(envio.Id);
				await ConsultarPlanes();
			}
			catch (Exception ex)
			{
				ViewBag.Error= ex.Message.ToString();
			}
			ViewBag.DocCargada = true;
			return View("Transferencia",data);
		}

		[HttpPost]
		public async Task<IActionResult> PrepararDocumentacion(Envio envio)
		{
			TablaGenerica data = new TablaGenerica();
			try
			{
				String accion = ReadConfig.ReadKey("AppServices", "ArmadoArchivos");
				ClasePeticion<Envio> peticion = new ClasePeticion<Envio>();
				String cveCarga=HttpContext.Session.GetString("IdCarga");
				envio.Id = int.Parse(cveCarga);
				peticion.Clase = envio;
				ClienteRest<RespuestaData<List<UniversalModeloNegocio.Generales.Arbol>>> cliente = new ClienteRest<RespuestaData<List<UniversalModeloNegocio.Generales.Arbol>>>();
				RespuestaData<List<UniversalModeloNegocio.Generales.Arbol>> respuesta = await cliente.LLamarServicioPostGeneral(apiUrlAON, accion, peticion);
				ViewBag.ArchivosArmado = respuesta.Datos;
				data = await leerLayout(envio.Id);
				if(respuesta!=null && respuesta.Respuesta != null && respuesta.Respuesta.result != 1)
                {
					ViewBag.ErrorArmado=respuesta.Respuesta.mensaje;
                }
                await ConsultarPlanes();
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message.ToString();
			}
			ViewBag.DocCargada = true;
			ViewBag.TipoEnvioConfirm = envio.tipoEnvio;
			return View("Transferencia", data);
		}

		private async Task<TablaGenerica> leerLayout(int Id)
		{
			TablaGenerica data = new TablaGenerica(); ;
			try
			{
				ClasePeticion<int> peticion = new ClasePeticion<int>();
				peticion.Clase = Id;
				ClienteRest<RespuestaData<TablaGenerica>> cliente = new ClienteRest<RespuestaData<TablaGenerica>>();
				RespuestaData<TablaGenerica> respuesta=await cliente.LLamarServicioPostGeneral(apiUrlAON, ReadConfig.ReadKey("AppServices", "LeerLayout"), peticion);
				data = respuesta.Datos;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return data;
		}

		public ActionResult Perfil(int idRol,int idSitio,int idSitioExterno,String NombreSitio)
		{
			MAC.Accesos.VMPerfilAcceso DataPerfiles = Newtonsoft.Json.JsonConvert.DeserializeObject<MAC.Accesos.VMPerfilAcceso>(HttpContext.Session.GetString("Roles"));
			String appid = HttpContext.Session.GetString("IddApNum");
			String IdUser = HttpContext.Session.GetString("IddNum");

			HttpContext.Session.SetString("RolActual", idRol.ToString());
			HttpContext.Session.SetString("SitioActual", idSitio.ToString());
			HttpContext.Session.SetString("SitioApp", idSitioExterno.ToString());
			HttpContext.Session.SetString("NombreSitio", NombreSitio);

			HttpContext.Session.SetString("MenuUsuario", Perfiles.MenuPerfil(DataPerfiles, idRol,idSitio));

			var tasksecciones = Task.Run(() => Perfiles.SesionSeccionesUsuario(idRol, int.Parse(IdUser), apiUrlLog));
			var valh = tasksecciones.Result;
			HttpContext.Session.SetString("SeccionesUsuario", valh);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> ConsultarMensajes(object o)
		{
			try
			{
				Respuesta serviceResult = new Respuesta();
				ClienteRest<Respuesta> cliente = new ClienteRest<Respuesta>();
				MensajeApp mensaje = new MensajeApp();
				int idSitio = int.Parse(HttpContext.Session.GetString("SitioActual"));
				List<int> ListaPerfiles = Perfiles.ListaPerfiles(HttpContext.Session.GetString("Roles"), idSitio);
				mensaje.id_Rol_Destino = ListaPerfiles[0];
				String app = HttpContext.Session.GetString("IddApNum");
				mensaje.id_App = int.Parse(app);
				mensaje.id_Tipo_Mensaje = 1; //mensajes
				serviceResult = await cliente.LLamarServicioPostGeneral(apiUrl, ReadConfig.ReadKey("AppServices", "ServicioAlertas"), mensaje);
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message.ToString();
			}
			return PartialView("~/Views/Shared/_TopNavbar.cshtml");
		}

		[HttpPost]
		public async Task<IActionResult> EliminarMensajes(MensajeApp data)
		{
			try
			{
				Respuesta serviceResult = new Respuesta();
				ClienteRest<Respuesta> cliente = new ClienteRest<Respuesta>();
				serviceResult = await cliente.LLamarServicioPutGeneral(apiUrlMensajeria, ReadConfig.ReadKey("AppServices", "ServicioEliminaMensajesApp"), data);
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message.ToString();
			}
			return PartialView("~/Views/Shared/_TopNavbar.cshtml");
		}

		[HttpPost]
		public async Task<JsonResult> EliminarMensaje(MensajeApp data)
		{
			RespuestaModel resp = new RespuestaModel();
			try
			{
				Respuesta serviceResult = new Respuesta();
				ClienteRest<Respuesta> cliente = new ClienteRest<Respuesta>();
				serviceResult = await cliente.LLamarServicioPutGeneral(apiUrlMensajeria, ReadConfig.ReadKey("AppServices", "ServicioEliminaMensajeApp"), data.id);
				resp.success = serviceResult.Status == 1 ? true : false;
				resp.mensaje = serviceResult.Mensaje;
			}
			catch (Exception ex)
			{
				resp.success = false;
				resp.mensaje = ex.Message;
			}
			return Json(new { model = resp });
		}

		[HttpPost]
		public async Task<IActionResult> LeerMensajes(MensajeApp data)
		{
			try
			{
				Respuesta serviceResult = new Respuesta();
				ClienteRest<Respuesta> cliente = new ClienteRest<Respuesta>();
				MensajeAPPExt mensaje = new MensajeAPPExt();
				mensaje.mensaje = new MensajeApp();
				mensaje.mensaje = data;
				mensaje.username = HttpContext.Session.GetString("IddPar");
				serviceResult = await cliente.LLamarServicioPutGeneral(apiUrlMensajeria, ReadConfig.ReadKey("AppServices", "ServicioLeerMensajesApp"), mensaje);
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message.ToString();
			}
			return PartialView("~/Views/Shared/_TopNavbar.cshtml");
		}

		[HttpPost]
		public async Task<JsonResult> LeerMensaje(MensajeApp data)
		{
			RespuestaModel resp = new RespuestaModel();
			try
			{
				if (data.jSON_Data == null) { data.jSON_Data = String.Empty; }
				data.isread = true;
				Respuesta serviceResult = new Respuesta();
				ClienteRest<Respuesta> cliente = new ClienteRest<Respuesta>();
				MensajeAPPExt mensaje = new MensajeAPPExt();
				mensaje.mensaje = new MensajeApp();
				mensaje.mensaje = data;
				if (!String.IsNullOrEmpty(mensaje.mensaje.jSON_Data) && mensaje.mensaje.jSON_Data.IndexOf("{") < 0) { mensaje.mensaje.jSON_Data = "{" + mensaje.mensaje.jSON_Data + "}"; }
				mensaje.username = HttpContext.Session.GetString("IddPar");
				serviceResult = await cliente.LLamarServicioPutGeneral(apiUrlMensajeria, ReadConfig.ReadKey("AppServices", "ServicioActualizarMensajeApp"), mensaje);
				resp.success = serviceResult != null ? true : false;
				resp.mensaje = String.Empty;
			}
			catch (Exception ex)
			{
				resp.success = false;
				resp.mensaje = ex.Message;
			}
			return Json(new { model = resp });
		}

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

		public IActionResult Login()
		{
			HttpContext.Session.SetString("AppToken", String.Empty);
			return RedirectToAction("Index", "Home");
		}
	}
}
