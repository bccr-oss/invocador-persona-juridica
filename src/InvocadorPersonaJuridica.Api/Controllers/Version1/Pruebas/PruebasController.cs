using System.Collections.Generic;
using System.Threading.Tasks;
using Abstracciones;
using Microsoft.AspNetCore.Mvc;

namespace InvocadorPersonaJuridica.Api
{
    [Route("{controller}")]
	[ApiVersion("1.0")]
	public class PruebasController : ControllerBase
	{
		private readonly IProveedorApiEndpoints _proveedorApiEndpoints;
		private readonly IRequestsHandler _requestHandler;

		public PruebasController(IProveedorApiEndpoints proveedorApiEndpoints, IRequestsHandler requestHandler)
		{
			_proveedorApiEndpoints = proveedorApiEndpoints;
			_requestHandler = requestHandler;
		}

		[HttpGet]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> Get()
		{
			var apiRuta = _proveedorApiEndpoints.ObtengaLaConfiguracion("Pruebas");
			var resultado = await _requestHandler.GetAsync<List<KeyValuePair<string, string>>>(apiRuta);

			return Ok(resultado.Respuesta);
		}
	}
}