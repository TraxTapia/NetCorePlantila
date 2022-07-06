using MACServiceGenerico.Respuesta;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MAC.Servicios.ClienteRestNC
{

	public class ClienteRest<T>
	{
		public async Task<string> ServicoGetTexto(String apiUrl, String recurso)
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					client.BaseAddress = new Uri(apiUrl);
					client.DefaultRequestHeaders.Clear();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					HttpResponseMessage Res = await client.GetAsync(recurso);
					String Response = Res.Content.ReadAsStringAsync().Result;
					if (Res.IsSuccessStatusCode)
					{
						var RespuestaData = Response;
						return RespuestaData;
					}
					else
					{
						throw new Exception(Response);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<RespuestaData<T>> LLamarServicio(String apiUrl, String recurso)
		{
			try
			{
				RespuestaData<T> RespuestaData = new RespuestaData<T>();
				using (HttpClient client = new HttpClient())
				{
					client.BaseAddress = new Uri(apiUrl);
					client.DefaultRequestHeaders.Clear();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					HttpResponseMessage Res = await client.GetAsync(recurso);
					String Response = Res.Content.ReadAsStringAsync().Result;
					if (Res.IsSuccessStatusCode)
					{
						RespuestaData = JsonConvert.DeserializeObject<RespuestaData<T>>(Response);
					}
					else
					{
						throw new Exception(Response);
					}
				}
				return RespuestaData;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<T> LLamarServicioSimple(String apiUrl, String recurso)
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					client.BaseAddress = new Uri(apiUrl);
					client.DefaultRequestHeaders.Clear();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					HttpResponseMessage Res = await client.GetAsync(recurso);
					String Response = Res.Content.ReadAsStringAsync().Result;
					if (Res.IsSuccessStatusCode)
					{
						var RespuestaData = JsonConvert.DeserializeObject<T>(Response);
						return RespuestaData;
					}
					else
					{
						throw new Exception(Response);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<RespuestaData<T>> LLamarServicio<T2>(String apiUrl, String recurso, T2 Params)
		{
			try
			{
				RespuestaData<T> RespuestaData = new RespuestaData<T>();
				StringBuilder myStringBuilder = new StringBuilder("?");
				foreach (var prop in Params.GetType().GetProperties())
				{
					myStringBuilder.Append(prop.Name + "=" + prop.GetValue(Params, null).ToString());
				}
				apiUrl += myStringBuilder.ToString();
				RespuestaData = await LLamarServicio(apiUrl, recurso);
				return RespuestaData;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<T> LLamarServicio_(String apiUrl, String recurso)
		{
			try
			{

				using (HttpClient client = new HttpClient())
				{
					client.BaseAddress = new Uri(System.Net.WebUtility.UrlEncode(apiUrl));
					client.DefaultRequestHeaders.Clear();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					HttpResponseMessage Res = await client.GetAsync(recurso);
					String Response = Res.Content.ReadAsStringAsync().Result;
					if (Res.IsSuccessStatusCode)
					{

						return JsonConvert.DeserializeObject<T>(Response);
					}
					else
					{
						throw new Exception(Response);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<T> LLamarServicioSimple<T2>(String apiUrl, String recurso, T2 Params)
		{
			try
			{
				StringBuilder myStringBuilder = new StringBuilder("?");
				foreach (var prop in Params.GetType().GetProperties())
				{
					myStringBuilder.Append(prop.Name + "=" + prop.GetValue(Params, null).ToString());
				}
				apiUrl += myStringBuilder.ToString();
				var RespuestaData = await LLamarServicioSimple(apiUrl, recurso);
				return RespuestaData;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<RespuestaData<T>> LLamarServicioJSon<T2>(String apiUrl, String recurso, T2 Params)
		{
			try
			{
				RespuestaData<T> RespuestaData = new RespuestaData<T>();
				StringBuilder myStringBuilder = new StringBuilder("?Filtro=");
				String JSon = JsonConvert.SerializeObject(Params);
				myStringBuilder.Append(JSon);
				apiUrl += myStringBuilder.ToString();
				RespuestaData = await LLamarServicio(apiUrl, recurso);
				return RespuestaData;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<RespuestaData<T>> LLamarServicioPost<T2>(String apiUrl, String recurso, T2 Params)
		{
			try
			{
				Uri requestUrl = CreateRequestUri(apiUrl, string.Format(System.Globalization.CultureInfo.InvariantCulture,
				recurso));
				RespuestaData<T> RespuestaData = new RespuestaData<T>();
				using (HttpClient client = new HttpClient())
				{
					client.DefaultRequestHeaders.Clear();
					HttpResponseMessage Res = await client.PostAsync(requestUrl.ToString(), CreateHttpContent<T2>(Params));
					String Response = Res.Content.ReadAsStringAsync().Result;
					if (Res.IsSuccessStatusCode)
					{
						RespuestaData = JsonConvert.DeserializeObject<RespuestaData<T>>(Response);
					}
					else
					{
						throw new Exception(Response);
					}
				}
				return RespuestaData;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<RespuestaSimple> LLamarServicioPostSimple<T2>(String apiUrl, String recurso, T2 Params)
		{
			try
			{
				Uri requestUrl = CreateRequestUri(apiUrl, string.Format(System.Globalization.CultureInfo.InvariantCulture,
				recurso));
				RespuestaSimple respuestaData = new RespuestaSimple();
				using (HttpClient client = new HttpClient())
				{
					client.DefaultRequestHeaders.Clear();
					HttpResponseMessage Res = await client.PostAsync(requestUrl.ToString(), CreateHttpContent<T2>(Params));
					String Response = Res.Content.ReadAsStringAsync().Result;
					if (Res.IsSuccessStatusCode)
					{

						respuestaData = JsonConvert.DeserializeObject<RespuestaSimple>(Response);
					}
					else
					{
						throw new Exception(Response);
					}
				}
				return respuestaData;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<T> LLamarServicioPostSimple2<T2>(String apiUrl, String recurso, T2 Params)
		{
			try
			{
				Uri requestUrl = CreateRequestUri(apiUrl, string.Format(System.Globalization.CultureInfo.InvariantCulture, recurso));
				using (HttpClient client = new HttpClient())
				{
					client.DefaultRequestHeaders.Clear();
					HttpResponseMessage Res = await client.PostAsync(requestUrl.ToString(), CreateHttpContent<T2>(Params));
					String Response = Res.Content.ReadAsStringAsync().Result;
					if (Res.IsSuccessStatusCode)
					{

						var RespuestaData = JsonConvert.DeserializeObject<T>(Response);
						return RespuestaData;
					}
					else
					{
						throw new Exception(Response);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<T> LLamarServicioPostSimple3<T2>(String apiUrl, String recurso, T2 Params)
		{
			try
			{
				Uri requestUrl = CreateRequestUri(apiUrl, string.Format(System.Globalization.CultureInfo.InvariantCulture, recurso));
				using (HttpClient client = new HttpClient())
				{
					client.DefaultRequestHeaders.Clear();
					HttpResponseMessage Res = await client.PostAsync(requestUrl.ToString(), CreateHttpContent<T2>(Params));
					String Response = Res.Content.ReadAsStringAsync().Result;
					if (Res.IsSuccessStatusCode)
					{

						var respuesta = JsonConvert.DeserializeObject<T>(Response);
						return respuesta;
					}
					else
					{
						throw new Exception(Response);
					}
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<RespuestaData<T>> LLamarServicioPut<T2>(String apiUrl, String recurso, T2 Params)
		{
			try
			{
				Uri requestUrl = CreateRequestUri(apiUrl, string.Format(System.Globalization.CultureInfo.InvariantCulture, recurso));
				RespuestaData<T> RespuestaData = new RespuestaData<T>();
				using (HttpClient client = new HttpClient())
				{
					client.DefaultRequestHeaders.Clear();
					HttpResponseMessage Res = await client.PutAsync(requestUrl.ToString(), CreateHttpContent<T2>(Params));
					String Response = Res.Content.ReadAsStringAsync().Result;
					if (Res.IsSuccessStatusCode)
					{
						RespuestaData = JsonConvert.DeserializeObject<RespuestaData<T>>(Response);
					}
					else
					{
						throw new Exception(Response);
					}
				}
				return RespuestaData;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<List<T>> LLamarServicioPostGeneralList<T2>(String apiUrl, String recurso, T2 Params)
		{
			try
			{
				Uri requestUrl = CreateRequestUri(apiUrl, string.Format(System.Globalization.CultureInfo.InvariantCulture, recurso));
				var RespuestaData = default(List<T>);
				using (HttpClient client = new HttpClient())
				{
					client.DefaultRequestHeaders.Clear();
					HttpResponseMessage Res = await client.PostAsync(requestUrl.ToString(), CreateHttpContent<T2>(Params));
					String Response = Res.Content.ReadAsStringAsync().Result;
					if (Res.IsSuccessStatusCode)
					{
						RespuestaData = JsonConvert.DeserializeObject<List<T>>(Response);
					}
					else
					{
						throw new Exception(Response);
					}
				}
				return RespuestaData;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<T> LLamarServicioPostGeneral<T2>(String apiUrl, String recurso, T2 Params)
		{
			try
			{
				Uri requestUrl = CreateRequestUri(apiUrl, string.Format(System.Globalization.CultureInfo.InvariantCulture, recurso));
				var RespuestaData = default(T);
				Double ms = 3600000.00D;
				using (HttpClient client = new HttpClient())
				{
					client.DefaultRequestHeaders.Clear();
					client.Timeout = TimeSpan.FromMilliseconds(ms);
					HttpResponseMessage Res = await client.PostAsync(requestUrl.ToString(), CreateHttpContent<T2>(Params));
					String Response = Res.Content.ReadAsStringAsync().Result;
					if (Res.IsSuccessStatusCode)
					{
						RespuestaData = JsonConvert.DeserializeObject<T>(Response);
					}
					else
					{
						throw new Exception(Response);
					}
				}
				return RespuestaData;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<T> LLamarServicioPutGeneral<T2>(String apiUrl, String recurso, T2 Params)
		{
			try
			{
				Uri requestUrl = CreateRequestUri(apiUrl, string.Format(System.Globalization.CultureInfo.InvariantCulture, recurso));
				var RespuestaData = default(T);
				using (HttpClient client = new HttpClient())
				{
					client.DefaultRequestHeaders.Clear();
					HttpResponseMessage Res = await client.PutAsync(requestUrl.ToString(), CreateHttpContent<T2>(Params));
					String Response = Res.Content.ReadAsStringAsync().Result;
					if (Res.IsSuccessStatusCode)
					{
						RespuestaData = JsonConvert.DeserializeObject<T>(Response);
					}
					else
					{
						throw new Exception(Response);
					}
				}
				return RespuestaData;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<List<T>> LLamarServicioPutGeneralList<T2>(String apiUrl, String recurso, T2 Params)
		{
			try
			{
				Uri requestUrl = CreateRequestUri(apiUrl, string.Format(System.Globalization.CultureInfo.InvariantCulture, recurso));
				var RespuestaData = default(List<T>);
				using (HttpClient client = new HttpClient())
				{
					client.DefaultRequestHeaders.Clear();
					HttpResponseMessage Res = await client.PutAsync(requestUrl.ToString(), CreateHttpContent<T2>(Params));
					String Response = Res.Content.ReadAsStringAsync().Result;
					if (Res.IsSuccessStatusCode)
					{
						RespuestaData = JsonConvert.DeserializeObject<List<T>>(Response);
					}
					else
					{
						throw new Exception(Response);
					}
				}
				return RespuestaData;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		private Uri CreateRequestUri(String BaseEndpoint, string relativePath, string queryString = "")
		{
			var uBaseEndpoint = new Uri(BaseEndpoint);
			var endpoint = new Uri(uBaseEndpoint, relativePath);
			var uriBuilder = new UriBuilder(endpoint);
			uriBuilder.Query = queryString;
			return uriBuilder.Uri;
		}
		private HttpContent CreateHttpContent<T2>(T2 content)
		{
			var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
			//var json = JsonConvert.SerializeObject(content);
			return new StringContent(json, Encoding.UTF8, "application/json");
		}
		private static JsonSerializerSettings MicrosoftDateFormatSettings
		{
			get
			{
				return new JsonSerializerSettings
				{
					DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
				};
			}
		}

	}
}
