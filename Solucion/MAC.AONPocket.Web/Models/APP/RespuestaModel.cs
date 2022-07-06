using System;
using System.Collections.Generic;

namespace MAC.AONPocket.Web.Models.APP
{
	public class RespuestaModel
	{
		public bool success { get; set; }
		private List<object> _results = new List<object>();
		public List<object> results { get => _results; set => _results = value; }
		public object result { get; set; }
		public string mensaje { get; set; }
		public string funcion { get; set; }
		public string href { get; set; }
		private String _alerta = String.Empty;
		public string alerta { get => _alerta; set => _alerta = value; }
		public string target { get; set; }
		public string html { get; set; }

		/// <summary>
		/// constructor
		/// </summary>
		public RespuestaModel()
		{
			this.success = false;
			this.mensaje = string.Empty;
		}

		public void SetRespuesta(bool respuesta, string msj = null)
		{
			if (!respuesta && msj == "")
			{
				mensaje = "Ocurrio un error inesperado";
			}
			else
			{
				this.mensaje = msj;
			}
			this.success = respuesta;
		}
	}
}
