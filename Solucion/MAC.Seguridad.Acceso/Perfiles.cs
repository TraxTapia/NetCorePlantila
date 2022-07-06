using MAC.Servicios.ClienteRestNC;
using MACServiceGenerico.Respuesta;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UniversalModeloNegocio.ExpedienteCE;

namespace MAC.Serguridad.Acceso
{
	public static class Perfiles
	{

		public static async Task<String[]> obtenerOpciones(int idApp, int idUsuario, String apiUrl)
		{
			String[] Perfil = { "", "", "", "", "", "" };
			String SMenu = String.Empty;
			String SPerfil = String.Empty;
			Menu ArbolPerfiles = new Menu();
			try
			{
				VMPeticionPerfil peticion = new VMPeticionPerfil();
				peticion.IdApp = idApp;
				peticion.IdUsuario = idUsuario;

				MAC.Accesos.VMPerfilAcceso DataPerfiles = await perfilesAcceso(apiUrl, peticion);
				Perfil[1] = Newtonsoft.Json.JsonConvert.SerializeObject(DataPerfiles);
				int idSitio = 0;
				int? idSitioApp = 0;
				int idRol = 0;
				String NombreSitio = String.Empty;
				if (DataPerfiles.ListSitioAcceso.Count > 0)
				{
					idSitio = DataPerfiles.ListSitioAcceso[0].IdSitio;
					idSitioApp = DataPerfiles.ListSitioAcceso[0].IdSitioExterno;
					NombreSitio = DataPerfiles.ListSitioAcceso[0].NombreSitio;
					if (DataPerfiles.ListSitioAcceso[0].ListRolAcceso.Count > 0)
					{
						idRol = DataPerfiles.ListSitioAcceso[0].ListRolAcceso[0].IdRol;
					}
				}
				SMenu = MenuPerfil(DataPerfiles, idRol, idSitio);
				Perfil[0] = SMenu;
				Perfil[2] = idRol.ToString();
				Perfil[3] = idSitio.ToString();
				Perfil[4] = NombreSitio;
				Perfil[5] = idSitioApp.ToString();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return Perfil;
		}

		public static String MenuPerfil(MAC.Accesos.VMPerfilAcceso DataPerfiles, int idRol, int idSitio)
		{
			String SMenu = String.Empty;
			try
			{
				Menu menu = new Menu();
				if (DataPerfiles.ListSitioAcceso.Count > 0)
				{
					int indexSitio = DataPerfiles.ListSitioAcceso.FindIndex(x => x.IdSitio == idSitio);
					int indexRol = DataPerfiles.ListSitioAcceso[indexSitio].ListRolAcceso.FindIndex(x => x.IdRol == idRol);
					if (indexRol >= 0)
					{
						foreach (MAC.Accesos.VMMapa<MAC.Accesos.VMModuloApp, MAC.Accesos.VMItemModulo> m in DataPerfiles.ListSitioAcceso[indexSitio].ListRolAcceso[indexRol].ListModulos)
						{
							menu.MenuContent.Opciones.Add(nuevomenu("MOD_" + m.Nodo.IdModulo.ToString(), "MOD_" + m.Nodo.IdModulo.ToString(), m.Nodo.NombreModulo, String.Empty, m.Nodo.UrlIcono != null ? m.Nodo.UrlIcono : String.Empty, String.Empty));
							foreach (MAC.Accesos.VMTree<MAC.Accesos.VMItemModulo> n in m.Nodos)
							{
								menu.MenuContent.Opciones.Add(nuevomenu("SM_" + n.Nodo.IdItemModulo.ToString(), n.Nodo.IdItemPadre == 0 ? "MOD_" + m.Nodo.IdModulo.ToString() : "SM_" + n.Nodo.IdItemPadre.ToString(), n.Nodo.DescripcionItem, "1", n.Nodo.UrlIcono != null ? n.Nodo.UrlIcono : String.Empty, n.Nodo.UrlDestino));
							}
						}
					}
				}
				SMenu = Newtonsoft.Json.JsonConvert.SerializeObject(menu);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return SMenu;
		}

		private static async Task<MAC.Accesos.VMPerfilAcceso> perfilesAcceso(String apiUrl, VMPeticionPerfil peticion)
		{
			String result = String.Empty;
			try
			{
				String accion = "UsuarioAcceso/Usuario/PerfilAcceso";
				ClienteRest<MAC.Accesos.VMPerfilAcceso> cliente = new ClienteRest<MAC.Accesos.VMPerfilAcceso>();
				MAC.Accesos.VMPerfilAcceso Data = await cliente.LLamarServicioPostGeneral<VMPeticionPerfil>(apiUrl, accion, peticion);
				return Data;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static async Task<String> SesionSeccionesUsuario(int idRol, int idUsuario, String apiUrl)
		{
			try
			{
				String SesionSecciones = String.Empty;
				MAC.Accesos.VMItemSeccion[] Secciones = await GetSeccionesUsuario(idRol, idUsuario, apiUrl);
				SesionSecciones = Newtonsoft.Json.JsonConvert.SerializeObject(Secciones);
				return SesionSecciones;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private static async Task<MAC.Accesos.VMItemSeccion[]> GetSeccionesUsuario(int idRol, int idUsuario, String apiUrl)
		{
			try
			{
				MAC.Accesos.VMItemSeccion[] Secciones = null;
				MAC.Accesos.VMUsuarioItemSeccionFiltros filtroS = new MAC.Accesos.VMUsuarioItemSeccionFiltros();
				filtroS.IdRol = idRol;
				filtroS.IdUsuario = idUsuario;
				filtroS.FlPermitir = true;
				MAC.Accesos.VMGridUsuarioItemSeccionGet filtro = new MAC.Accesos.VMGridUsuarioItemSeccionGet();
				filtro.Filtro = filtroS;
				filtro.Todas = true;
				filtro.Descendente = false;

				VMTable<MAC.Accesos.VMItemSeccion> tabla = new UniversalModeloNegocio.ExpedienteCE.VMTable<MAC.Accesos.VMItemSeccion>();

				String accion = "Login/UsuarioItemSeccionDetalle/Grid";
				ClienteRest<VMTable<MAC.Accesos.VMItemSeccion>> cliente = new ClienteRest<VMTable<MAC.Accesos.VMItemSeccion>>();
				tabla = await cliente.LLamarServicioPostGeneral<MAC.Accesos.VMUsuarioItemSeccionFiltros>(apiUrl, accion, filtroS);

				Secciones = tabla.Lista.ToArray();
				return Secciones;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private static ElementoMenu nuevomenu(String pid, String ppadre, string pTexto, String pTipo, string picono, String pagina)
		{
			ElementoMenu elm = new ElementoMenu();
			elm.id = pid;
			elm.father = ppadre;
			elm.texto = pTexto;
			elm.tipo = pTipo;
			elm.accion = pagina;
			elm.imagen = picono;
			return elm;
		}

		public static List<int> ListaPerfiles(string pPerfiles, int idSitio)
		{
			List<int> LPerfiles = new List<int>();
			MAC.Accesos.VMPerfilAcceso DataPerfiles = Newtonsoft.Json.JsonConvert.DeserializeObject<MAC.Accesos.VMPerfilAcceso>(pPerfiles);
			int indexSitio = DataPerfiles.ListSitioAcceso.FindIndex(x => x.IdSitio == idSitio);
			foreach (MAC.Accesos.VMRolAcceso pa in DataPerfiles.ListSitioAcceso[indexSitio].ListRolAcceso)
			{
				LPerfiles.Add(pa.IdRol);
			}
			return LPerfiles;
		}
	}
}
