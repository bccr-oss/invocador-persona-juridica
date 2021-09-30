using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Abstracciones;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InvocadorPersonaJuridica.Api
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class RequestHelper 
	{
		/// <summary>
		/// 
		/// </summary>
		protected ILogger _logger;

		/// <summary>
		/// 
		/// </summary>
		protected IConfiguration _configuracion;

		/// <summary>
		/// 
		/// </summary>
		protected IHostEnvironment _env;

		/// <summary>
		/// 
		/// </summary>
		protected IHttpContextAccessor _httpContextAccessor;

		/// <summary>
		/// 
		/// </summary>
		protected IApiEndpoint _apiRuta;

		/// <summary>
		/// 
		/// </summary>
		protected string _method;

		/// <summary>
		/// 
		/// </summary>
		private readonly IProveedorJsonWebToken _proveedorJsonWebToken;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		protected RequestHelper(HttpContext context)
		{
			_env ??= context.RequestServices.GetRequiredService<IHostEnvironment>();
			_logger ??= context.RequestServices.GetRequiredService<ILogger<RequestsHandler>>();
			_proveedorJsonWebToken ??= context.RequestServices.GetRequiredService<IProveedorJsonWebToken>();
			_configuracion ??= context.RequestServices.GetRequiredService<IConfiguration>();
			_httpContextAccessor ??= context.RequestServices.GetRequiredService<IHttpContextAccessor>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="apiRuta"></param>
		/// <param name="method"></param>
		/// <returns></returns>
		protected async Task<HttpWebRequest> ObtenerElWebRequest(IApiEndpoint apiRuta, string method)
		{
			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(apiRuta.Ruta);
			webRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			webRequest.Headers.Add("X-Correlation-ID", $"{ Trace.CorrelationManager.ActivityId }");
			webRequest.Method = method;
			webRequest.Timeout = apiRuta.TimeOut;
			webRequest.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {await _proveedorJsonWebToken.ProveaElTokenDeAcceso()}");

			return webRequest;
		}
	}
}
