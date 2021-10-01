using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Abstracciones;
using Abstracciones.Contenedores;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace InvocadorPersonaJuridica.Api
{
    /// <summary>
    /// 
    /// </summary>
    internal class RequestSimple : RequestHelper
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="apiRuta"></param>
		/// <param name="method"></param>
		/// <param name="proveedorDelCertificadoCliente"></param>
		public RequestSimple(HttpContext context, IApiEndpoint apiRuta, string method)
			: base(context)
		{
			_apiRuta = apiRuta;
			_method = method;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="webRequest"></param>
		/// <returns></returns>
		private async Task<IResponse<T>> InvocarApi<T>(HttpWebRequest webRequest)
		{
			string respuesta;
			using (var webResponse = await webRequest.GetResponseAsync())
			{
				// Get the stream containing content returned by the server.  
				var responseStream = webResponse.GetResponseStream();
				// Open the stream using a StreamReader for easy access.  
				using (StreamReader reader = new StreamReader(responseStream))
				{
					// Read the content.  
					respuesta = await reader.ReadToEndAsync();
					reader.Close();
				}
				responseStream.Close();
				webResponse.Close();
			}

			IResponse<T> resultado = new Response<T> ()
			{
				Succeeded = true,
				Respuesta = string.IsNullOrEmpty(respuesta) ? default : JsonConvert.DeserializeObject<T>(respuesta)
			};

			return resultado;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="webRequest"></param>
		/// <returns></returns>
		private async Task<IResponse<T>> InvocarSinManejoDeErrores<T>(HttpWebRequest webRequest)
		{
			IResponse<T> resultado;

			try
			{
				resultado = await InvocarApi<T>(webRequest);
			}
			catch (Exception wex)
			{
				_logger.LogError($"Error consultando el endpoint [{_apiRuta.Nombre}] {Environment.NewLine}" +
								 $"Detalle: {wex.Message}");
				throw;
			}

			return resultado;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="webRequest"></param>
		/// <returns></returns>
		private async Task<IResponse<T>> InvocarConManejoDeErrores<T>(HttpWebRequest webRequest)
		{
			IResponse<T> resultado;

			try
			{
				resultado = await InvocarApi<T>(webRequest);
			}
			catch (Exception wex)
			{
				_logger.LogError($"Error: consultando el endpoint [{_apiRuta.Nombre}] {Environment.NewLine}" +
								 $"Detalle: {wex.Message}");

				resultado = new Response<T>()
				{
					Succeeded = false,
					Description = wex.Message
				};
			}

			return resultado;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		internal async Task<IResponse<T>> ExecuteSinManejoDeErrores<T>()
		{
			var webRequest = await ObtenerElWebRequest(_apiRuta, _method);
			return await InvocarSinManejoDeErrores<T>(webRequest);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		internal async Task<IResponse<T>> ExecuteConManejoDeErrores<T>()
		{
			var webRequest = await ObtenerElWebRequest(_apiRuta, _method);
			return await InvocarConManejoDeErrores<T>(webRequest);
		}
	}
}
