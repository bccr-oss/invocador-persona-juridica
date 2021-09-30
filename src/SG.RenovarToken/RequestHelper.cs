using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Abstracciones;
using Abstracciones.Contenedores;
using Newtonsoft.Json;
using SG.Autorizador.Utilitarios;

namespace SG.RenovarToken
{

	public class RequestHelper
	{
		private readonly IApiEndpoint _apiRuta;
		private readonly string _certificadoCliente;

		/// <summary>
		/// Contenedor de las cookies emitidas por el servidor de autorización.
		/// </summary>
		private readonly CookieContainer _cookieContainer;

		public RequestHelper(IApiEndpoint apiRuta, string certificadoCliente)
		{
			_apiRuta = apiRuta;
			_cookieContainer = new CookieContainer();
			_certificadoCliente = certificadoCliente;
		}

		public HttpWebRequest ObtenerElWebRequest()
		{
			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(_apiRuta.Ruta);
			webRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			webRequest.Headers.Add("X-Correlation-ID", $"{ Trace.CorrelationManager.ActivityId }");
			webRequest.Timeout = _apiRuta.TimeOut;
			webRequest.Method = WebRequestMethods.Http.Post;
			webRequest.ClientCertificates.Add(ObtenerElCertificadoCliente(_certificadoCliente));

			return webRequest;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="subject"></param>
		/// <returns></returns>
		private X509Certificate2 ObtenerElCertificadoCliente(string certificadoCliente)
		{
			X509Certificate x509Certificate = null;
			using (var handler = new CertificateHelper(StoreName.My, StoreLocation.LocalMachine))
			{
				x509Certificate = handler.GetCertificateBySubject(certificadoCliente);
			}

			if (x509Certificate == null)
				throw new ArgumentException($"El certificado [{certificadoCliente}] no se encuentra registrado en el store local.");

			return new X509Certificate2(x509Certificate);
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
			var parametros = Serializador.Serialice(request);
			byte[] dataStream = Encoding.UTF8.GetBytes(parametros);

			webRequest.ContentType = "application/x-www-form-urlencoded";
			webRequest.ContentLength = dataStream.Length;
			webRequest.CookieContainer = _cookieContainer;

			// Send the data.
			using (Stream requestStream = await webRequest.GetRequestStreamAsync())
			{
				await requestStream.WriteAsync(dataStream, 0, dataStream.Length);
				requestStream.Close();
			}

			using (var webResponse = (HttpWebResponse)await webRequest.GetResponseAsync())
			{
				foreach (Cookie cook in webResponse.Cookies)
				{
					_cookieContainer.Add(cook);
				}

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

		internal async Task<Token> ObtenerElToken(Parametros parametros)
		{
			var request = ObtenerElWebRequest();
			var resultado = await InvocarApi<Token, Parametros>(request, parametros);

			return resultado.Respuesta;
		}
	}
}
