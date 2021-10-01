using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Abstracciones;
using Abstracciones.Contenedores;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace InvocadorPersonaJuridica.Api
{
    internal class RequestComplejo : RequestHelper
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="apiRuta"></param>
		/// <param name="method"></param>
		public RequestComplejo(HttpContext context, IApiEndpoint apiRuta, string method)
			: base(context)
		{
			_apiRuta = apiRuta;
			_method = method;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="webRequest"></param>
		/// <param name="request"></param>
		/// <returns></returns>
		private async Task<IResponse<T>> InvocarApi<T, E>(HttpWebRequest webRequest, E request)
		{
			string respuesta;
			var parametros = JsonConvert.SerializeObject(request);
			byte[] dataStream = Encoding.UTF8.GetBytes(parametros);

			webRequest.PreAuthenticate = true;
			webRequest.ContentType = "application/json";
			webRequest.ContentLength = dataStream.Length;

			// Send the data.
			using (Stream requestStream = await webRequest.GetRequestStreamAsync())
			{
				await requestStream.WriteAsync(dataStream, 0, dataStream.Length);
				requestStream.Close();
			}

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

			IResponse<T> resultado = new Response<T>()
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
		/// <param name="request"></param>
		/// <returns></returns>
		private async Task<IResponse<T>> InvocarSinManejoDeErrores<T, E>(HttpWebRequest webRequest, E request)
		{
			IResponse<T> resultado;

			try
			{
				resultado = await InvocarApi<T, E>(webRequest, request);
			}
			catch (Exception wex)
			{
				_logger.LogError($"Error consultando el endpoint [{_apiRuta.Nombre}] {Environment.NewLine}" +
								 $"{wex.Message}");
				throw;
			}

			return resultado;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="webRequest"></param>
		/// <param name="request"></param>
		/// <returns></returns>
		private async Task<IResponse<T>> InvocarConManejoDeErrores<T, E>(HttpWebRequest webRequest, E request)
		{
			IResponse<T> resultado;

			try
			{
				resultado = await InvocarApi<T, E>(webRequest, request);
			}
			catch (Exception wex)
			{
				_logger.LogError($"Error consultando el endpoint [{_apiRuta.Nombre}] {Environment.NewLine}" +
								 $"{wex.Message}");

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
		/// <typeparam name="E"></typeparam>
		/// <param name="elemento"></param>
		/// <returns></returns>
		internal async Task<IResponse<T>> ExecuteSinManejoDeErrores<T, E>(E elemento)
		{
			var webRequest = await ObtenerElWebRequest(_apiRuta, _method);
			return await InvocarSinManejoDeErrores<T, E>(webRequest, elemento);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="E"></typeparam>
		/// <param name="elemento"></param>
		/// <returns></returns>
		internal async Task<IResponse<T>> ExecuteConManejoDeErrores<T, E>(E elemento)
		{
			var webRequest = await ObtenerElWebRequest(_apiRuta, _method);
			return await InvocarConManejoDeErrores<T, E>(webRequest, elemento);
		}
	}
}