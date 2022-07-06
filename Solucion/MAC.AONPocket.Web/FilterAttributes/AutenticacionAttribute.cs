using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MAC.Serguridad.Acceso;

namespace MAC.AONPocket.Web.App_Code
{
	public class AutenticacionAttribute : ActionFilterAttribute
	{
		private String urlLogService = String.Empty;
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			String Token = String.Empty;
			String url = String.Empty;
			String usuario = String.Empty;
			String appid = String.Empty;
			String lstError = String.Empty;
			Boolean validateToken = false;
			int minutosLogeo = 0;
            try
            {
				if (!String.IsNullOrEmpty(filterContext.HttpContext.Session.GetString("AppToken")))
				{
					Token = filterContext.HttpContext.Session.GetString("AppToken");
				}
				else
				{
					String parameter = filterContext.HttpContext.Request.Query["tkn"];
					if (parameter == null)
					{
						parameter = String.Empty;
					}
					else
					{
						validateToken = true;
						Token = parameter;
					}
					IConfiguration setting = ConfigHelper.GetConfig();

					IConfiguration servicesetting = setting.GetSection("LogService");
					url = servicesetting["url"];
					urlLogService = servicesetting["resturl"];
					usuario = servicesetting["usr"];
					appid = servicesetting["appId"];
					filterContext.HttpContext.Session.SetString("IddApNum", appid);
					minutosLogeo = Int32.Parse(servicesetting["delay"]);
				}

				if (!Token.Equals(String.Empty))
				{
					try
					{
						String Tokendec = Utilidades.Encriptacion.Desencriptar(Convert.FromBase64String(Token), usuario);
						if (validateToken)
						{
							String[] ADATATkn = Tokendec.Split("@");
							String sdateresultToken = ADATATkn[0];

							DateTime dateresultToken = DateTime.Now;
							DateTime daterequestToken = DateTime.Now;

							sdateresultToken = sdateresultToken.Replace("T", " ");
							dateresultToken = DateTime.ParseExact(sdateresultToken, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);

							daterequestToken = DateTime.ParseExact(filterContext.HttpContext.Session.GetString("DateToken"), "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);

							TimeSpan ts = dateresultToken - daterequestToken;

							int differenceInMinutes = ts.Minutes; // This is in int
							if (differenceInMinutes < minutosLogeo)
							{
								Perfil(filterContext, appid, ADATATkn[1], ADATATkn[2], ADATATkn[3], Token);
							}
							else
							{
								Token = String.Empty;
								//Se vuelve a inicializar la variable para otro login. 
								filterContext.HttpContext.Session.SetString("DateToken", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
							}
						}
						else
						{
							string baseUrl = UrlBase(filterContext.HttpContext.Request);
							String requestURL = Url(filterContext.HttpContext.Request);
							//TODO: URLS post o get que tendra permiso el usuario.
							List<String> UrlsEventos = new List<String>();
							bool tieneAcceso = permisos(filterContext.HttpContext.Session.GetString("MenuUsuario"), baseUrl, requestURL, UrlsEventos);
							if (!tieneAcceso)
							{
								filterContext.Result = new JsonResult(new { message = "No autorizado." });

							}
						}
					}
					catch (Exception ex)
					{
						lstError = ex.Message.ToString();
						Token = String.Empty;
					}
				}

				if (Token.Equals(String.Empty))
				{
					Byte[] appData = Utilidades.Encriptacion.Encriptar(appid, usuario);
					string appidenc = System.Net.WebUtility.UrlEncode(Convert.ToBase64String(appData));
					url = String.Concat(url, "?", "usr=", usuario, "&dest=", appidenc);
					filterContext.HttpContext.Session.SetString("DateToken", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
					if (!System.Diagnostics.Debugger.IsAttached)
					{
						filterContext.Result = new RedirectResult(url);
					}
					else
					{
						if (String.IsNullOrEmpty(filterContext.HttpContext.Session.GetString("NombreUsuario")))
						{
							String idUser = "23";
							String username = "klemus";
							String user = "Karen Giselle Lemus Bustamante";
							Perfil(filterContext, appid, idUser, username, user, String.Empty);
						}
					}
				}
			}
			catch (Exception ex)
			{
				filterContext.Result = new JsonResult(new { message = "Error: " + ex.Message.ToString() });
			}
		}

		private void Perfil(ActionExecutingContext filterContext, String appid, String IdUser, String UserName, String Usuario, String Token)
		{
			try
			{
				filterContext.HttpContext.Session.SetString("IddNum", IdUser);
				filterContext.HttpContext.Session.SetString("IddPar", UserName);
				filterContext.HttpContext.Session.SetString("NombreUsuario", Usuario);
				filterContext.HttpContext.Session.SetString("AppToken", Token);

				var g = Task.Run(() => Perfiles.obtenerOpciones(Convert.ToInt32(appid), Convert.ToInt32(IdUser), urlLogService));

				var val = g.Result;

				filterContext.HttpContext.Session.SetString("MenuUsuario", val[0]);
				filterContext.HttpContext.Session.SetString("Roles", val[1]);
				filterContext.HttpContext.Session.SetString("RolActual", val[2]);
				filterContext.HttpContext.Session.SetString("SitioActual", val[3]);
				filterContext.HttpContext.Session.SetString("NombreSitio", val[4]);
				filterContext.HttpContext.Session.SetString("SitioApp", val[5]);
				filterContext.HttpContext.Session.SetString("AppHeaderData", String.Empty);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Returns the absolute url.
		/// </summary>
		public string Url(HttpRequest request)
		{
			return $"{request.Scheme}://{request.Host}{request.Path}";
		}

		/// <summary>
		/// Returns the absolute url base.
		/// </summary>
		public string UrlBase(HttpRequest request)
		{
			return $"{request.Scheme}://{request.Host}";
		}

		private Boolean permisos(String Listapermisos, String baseURL, String Url, List<String> UrlsEvents)
		{
			//Aquí se valida que el usuario tenga permisos a la opción con el arreglo de urls.
			Listapermisos = Listapermisos.ToLower();
			baseURL = baseURL.ToLower();
			Url = Url.ToLower();
			String compareURL = baseURL;
			if (!compareURL.Substring(compareURL.Length - 1).Equals("/")) { compareURL = String.Concat(compareURL, "/"); }
			if (Url.Equals(compareURL)) { return true; }

			int posInicial = Url.IndexOf(baseURL) + baseURL.Length;
			Url = Url.Substring(posInicial);
			Url.Replace("~", "");
			if (Url.Equals("/home") || Url.Equals("/home/index")) { return true; }

			int count = Url.Count(f => f == '/');
			if (count == 1) { Url = String.Concat(Url, "/index"); }

			string result = UrlsEvents.FirstOrDefault(x => x == Url);
			//if (result != null)
			//{
			//	return true;
			//}

			/// Test
			int a = 1;
			if (a == 1)
			{
				return true;
			}
			////

			Menu menu = new Menu();
			menu = Newtonsoft.Json.JsonConvert.DeserializeObject<Menu>(Listapermisos);
			foreach (ElementoMenu elm in menu.MenuContent.Opciones)
			{
				elm.accion = elm.accion.Replace("~", "");
				count = elm.accion.Count(f => f == '/');
				if (count == 1) { elm.accion = String.Concat(elm.accion, "/index"); }
				if (elm.accion.Equals(Url))
				{
					return true;
				}
			}
			return false;
		}
	}
}

