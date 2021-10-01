using System.Net;
using System.Threading.Tasks;
using Abstracciones;
using Microsoft.AspNetCore.Http;

namespace InvocadorPersonaJuridica.Api
{
	/// <summary>
	/// 
	/// </summary>
	public class RequestsHandler : IRequestsHandler
    {
		/// <summary>
		/// 
		/// </summary>
		protected IHttpContextAccessor _httpContextAccessor;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="httpContextAccessor"></param>
		public RequestsHandler(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="apiRuta"></param>
		/// <returns></returns>
		public async Task<IResponse<T>> GetAsync<T>(IApiEndpoint apiRuta)
		{
			var get = new RequestSimple(_httpContextAccessor.HttpContext, apiRuta, WebRequestMethods.Http.Get);
			var response = await get.ExecuteSinManejoDeErrores<T>();

			return response;
		}

		/// <summary>
		/// Envía una solicitud GET al URI especificado en el objecto ApiRuta como una operación asincrónica, al cual se adjuntan los parametros serializados
		/// </summary>
		/// <typeparam name="T">Tipo</typeparam>
		/// <typeparam name="E">Elemento</typeparam>
		/// <param name="apiRuta">Objecto que contiene el API Endpoint</param>
		/// <param name="elemento">Parmetros en el query string de el endpoint, antes del ?</param>
		/// <returns>string con el resultado del request</returns>
		public async Task<IResponse<T>> GetAsync<T, E>(IApiEndpoint apiRuta, E elemento)
		{
			apiRuta.Ruta += $"&{Serializador.Serialice(elemento)}";
			var get = new RequestSimple(_httpContextAccessor.HttpContext, apiRuta, WebRequestMethods.Http.Get);
			var response = await get.ExecuteSinManejoDeErrores<T>();

			return response;
		}

		/// <summary>
		/// Envía una solicitud POST al URI especificado en el objecto ApiRuta como una operación asincrónica.
		/// </summary>
		/// <typeparam name="T">Tipo</typeparam>
		/// <typeparam name="E">Elemento</typeparam>
		/// <param name="apiRuta">Objecto que contiene el API Endpoint</param>
		/// <param name="elemento">Parmetros en el query string de el endpoint, antes del ?</param>
		/// <returns>string con el resultado del request</returns>
		public async Task<IResponse<T>> PostAsync<T, E>(IApiEndpoint apiRuta, E elemento)
		{
			var post = new RequestComplejo(_httpContextAccessor.HttpContext, apiRuta, WebRequestMethods.Http.Post);
			var response = await post.ExecuteSinManejoDeErrores<T, E>(elemento);

			return response;
        }

		/// <summary>
		/// Envía una solicitud PUT al URI especificado en el objecto ApiRuta como una operación asincrónica.
		/// </summary>
		/// <typeparam name="T">Tipo</typeparam>
		/// <typeparam name="E">Elemento</typeparam>
		/// <param name="apiRuta">Objecto que contiene el API Endpoint</param>
		/// <param name="elemento">Parmetros en el query string de el endpoint, antes del ?</param>
		/// <returns>string con el resultado del request</returns>
		public async Task<IResponse<T>> PutAsync<T, E>(IApiEndpoint apiRuta, E elemento)
        {
			var put = new RequestComplejo(_httpContextAccessor.HttpContext, apiRuta, WebRequestMethods.Http.Put);
			var response = await put.ExecuteSinManejoDeErrores<T, E>(elemento);

			return response;
		}

		/// <summary>
		/// Envía una solicitud DELETE al URI especificado en el objecto ApiRuta como una operación asincrónica.
		/// </summary>
		/// <param name="apiRuta">Objecto que contiene el API Endpoint</param>
		/// <returns>string con el resultado del request</returns>
		public async Task<IResponse<T>> DeleteAsync<T>(IApiEndpoint apiRuta)
		{
			var delete = new RequestSimple(_httpContextAccessor.HttpContext, apiRuta, "DELETE");
			var response = await delete.ExecuteSinManejoDeErrores<T>();

			return response;
		}

		/// <summary>
		/// Envía una solicitud DELETE al URI especificado en el objecto ApiRuta como una operación asincrónica.
		/// </summary>
		/// <param name="apiRuta">Objecto que contiene el API Endpoint</param>
		/// <param name="elemento">Parmetro en el query string de el endpoint, antes del ?</param>
		/// <returns>string con el resultado del request</returns>
		public async Task<IResponse<T>> DeleteAsync<T, E>(IApiEndpoint apiRuta, E elemento)
		{
			apiRuta.Ruta += $"&{Serializador.Serialice(elemento)}";
			var delete = new RequestSimple(_httpContextAccessor.HttpContext, apiRuta, "DELETE");
			var response = await delete.ExecuteSinManejoDeErrores<T>();

			return response;
		}
	}
}
