using System;
using System.Threading.Tasks;
using Abstracciones;
using Abstracciones.Contenedores;
using Abstracciones.SG;
using Microsoft.Extensions.Configuration;

namespace SG.RenovarToken
{
    /// <summary>
    /// Helper to perodically cleanup expired AuthenticationTicket.
    /// </summary>
    public class GestorDelToken : IGestorDelToken
	{
		private readonly IProveedorApiEndpoints _proveedorApiEndpoints;
		private readonly IProveedorDelSuscriptor _proveedorDelSuscriptor;
		private readonly IConfiguration _configuration;
		private readonly IGestorDeCache _gestorDeCache;

		/// <summary>
		/// Constructor for AuthenticationTicketCleanup.
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <param name="logger"></param>
		/// <param name="options"></param>
		public GestorDelToken(IConfiguration configuration,
							  IProveedorApiEndpoints proveedorApiEndpoints,
							  IGestorDeCache gestorDeCache,
							  IProveedorDelSuscriptor proveedorDelSuscriptor)
		{
			_configuration = configuration;
			_proveedorApiEndpoints = proveedorApiEndpoints;
			_gestorDeCache = gestorDeCache ?? throw new ArgumentNullException(nameof(gestorDeCache));
			_proveedorDelSuscriptor = proveedorDelSuscriptor ?? throw new ArgumentNullException(nameof(proveedorDelSuscriptor));
		}

		/// <summary>
		/// Method to clear expired AuthenticationTicket.
		/// </summary>
		/// <returns></returns>
		public async Task<Token> ObtieneElTokenAsync()
		{
			// 1 -> Obtener parámetros para consultar el autorizador según el parámetro (querystring) del suscriptor
			var parametros = _configuration.GetSection($"Suscriptores:{_proveedorDelSuscriptor.GetSuscriptor()}").Get<Parametros>();

            if (parametros is null)
				throw new ArgumentException($"Los parámetros correspondientes para el suscriptor [{_proveedorDelSuscriptor.GetSuscriptor()}], no se encuentra registrado la sección de suscriptores, revise el archivo appsettings.json");

			// 2 -> Obtiene el endpoint del autorizador de persona jurídica
			var apiRuta = _proveedorApiEndpoints.ObtengaLaConfiguracion("AutorizadorPersonaJuridica");

			// 3 -> Construir request
			var request = new RequestHelper(apiRuta, parametros.CertificadoCliente);

			// 4 -> Obtiene el token desde el autorizador.
			Token elToken = await request.ObtenerElToken(parametros);

			// Se establece el tiempo de vida del token a un 90 % de lo manejado por el  servidor.
			// Para no llegar a enviar un token vencido.
			elToken.Expires_Time = DateTime.Now.AddSeconds(elToken.expires_in*0.9);

			// 5 -> Almacena el token en el caché
			await _gestorDeCache.AlmacenaValorEnCache($"{_proveedorDelSuscriptor.GetSuscriptor()}_Token", elToken);

			return elToken;
		}
	}
}