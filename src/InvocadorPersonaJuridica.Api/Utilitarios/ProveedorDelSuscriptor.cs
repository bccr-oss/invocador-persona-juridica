using System;
using Abstracciones;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace InvocadorPersonaJuridica.Api
{
	/// <summary>
	/// Permite obtener el suscriptor que consume el API.
	/// </summary>
	public class ProveedorDelSuscriptor : IProveedorDelSuscriptor
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		private readonly IConfiguration _configuration;

		private string _elSuscriptor;

		/// <summary>
		/// Inicializa una nueva instancia de <see cref="ProveedorDelSuscriptor"/>
		/// </summary>
		/// <param name="httpContextAccessor"><see cref="IHttpContextAccessor"/></param>
		/// <param name="configuration"></param>
		public ProveedorDelSuscriptor(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
		{
			_httpContextAccessor = httpContextAccessor;
			_configuration = configuration;
		}

		/// <summary>
		/// Obtiene la versión del API que se consulta.
		/// </summary>
		/// <returns></returns>
		public ApiVersion GetVersion()
		{
			ApiVersion version = null;

			if (_httpContextAccessor.HttpContext != null)
				version = _httpContextAccessor.HttpContext.GetRequestedApiVersion();

			return version ?? new ApiVersion(1, 0);
		}

		/// <summary>
		/// Retorna el nombre del suscriptor del API.
		/// </summary>
		/// <returns>El nombre del suscriptor del API.</returns>
		public string GetSuscriptor()
		{
			ObtenerElSuscriptorDesdeElQueryString();
			ObtenerElSuscriptorDesdeLosHeaders();
			ObtenerElSuscriptorDesdeElFormData();
			ObtenerElSuscriptorPorOmision();

			return _elSuscriptor;
		}

		private void ObtenerElSuscriptorDesdeElQueryString()
		{
			if (_httpContextAccessor.HttpContext != null)
			{
				var suscriptor = _httpContextAccessor.HttpContext.Request.Query["suscriptor"];
				_elSuscriptor = suscriptor;
			}
		}

		private void ObtenerElSuscriptorDesdeLosHeaders()
		{
			if (string.IsNullOrEmpty(_elSuscriptor) && (_httpContextAccessor.HttpContext != null))
				_elSuscriptor = _httpContextAccessor.HttpContext.Request.Headers["X-Suscriptor"];
		}

		private void ObtenerElSuscriptorDesdeElFormData()
		{
			if (string.IsNullOrEmpty(_elSuscriptor) && (_httpContextAccessor.HttpContext != null) && _httpContextAccessor.HttpContext.Request.HasFormContentType)
				_elSuscriptor = _httpContextAccessor.HttpContext.Request.Form["X-Suscriptor"];
		}

		private void ObtenerElSuscriptorPorOmision()
		{
			if (string.IsNullOrEmpty(_elSuscriptor))
			{
				var suscriptorSection = _configuration.GetSection("Suscriptor");

                if (suscriptorSection is null)
					throw new ArgumentException($"No existe registrada la sección [Suscriptor] en el archivo de configuración appsettings.json");
			}
		}
	}
}