using System;
using System.Collections.Generic;
using System.Linq;
using Abstracciones;
using Abstracciones.Contenedores;
using Microsoft.Extensions.Configuration;

namespace InvocadorPersonaJuridica.Api
{
	/// <summary>
	/// 
	/// </summary>
	public class ProveedorApiEndpoints : IProveedorApiEndpoints
	{
		private const string apiEndpointsSection = "ApiEndpoints";

		private readonly IList<ApiEndpoint> _apiEndPoints;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="laConfiguracion"></param>
		public ProveedorApiEndpoints(IConfiguration laConfiguracion)
		{
			var lasApisPorAplicacion = laConfiguracion.GetSection(apiEndpointsSection).Get<IList<ApiEndpoint>>();

			List<ApiEndpoint> EndPoints;

			EndPoints = lasApisPorAplicacion.ToList() ?? throw new NullReferenceException($"No se encuentra registrada la sección de [{apiEndpointsSection}], revise el archivo settings.json");

			_apiEndPoints = EndPoints;

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="elNombre"></param>
		/// <returns></returns>
		public int ObtengaElTimeOutDelRequest(string elNombre)
		{
			int elResultado = _apiEndPoints.FirstOrDefault(a => a.Nombre.Equals(elNombre, StringComparison.OrdinalIgnoreCase)).TimeOut;

			return elResultado;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="elNombre"></param>
		/// <returns></returns>
		public IApiEndpoint ObtengaLaConfiguracion(string elNombre)
		{
			ApiEndpoint apiRuta = _apiEndPoints.FirstOrDefault(a => a.Nombre.Equals(elNombre, StringComparison.OrdinalIgnoreCase));

			if (apiRuta is null)
				throw new ArgumentException($"El endpoint [{elNombre}], no se encuentra registrado la sección [{apiEndpointsSection}], revise el archivo appsettings.json");

			return apiRuta.Clone() as ApiEndpoint;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="elNombre"></param>
		/// <returns></returns>
		public string ObtengaLaRutaDelApi(string elNombre)
		{
			string elResultado = _apiEndPoints.FirstOrDefault(a => a.Nombre.Equals(elNombre, StringComparison.OrdinalIgnoreCase)).Ruta;

			return elResultado;
		}
	}
}
