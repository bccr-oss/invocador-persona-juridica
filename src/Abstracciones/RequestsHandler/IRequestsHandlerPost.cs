using System.Threading.Tasks;

namespace Abstracciones
{
	/// <summary>
	/// Abstracción para registrar en el IOC la concreción encargada de realizar los requests a las api's.
	/// </summary>
	public interface IRequestsHandlerPost
	{
		/// <summary>
		/// Envía una solicitud POST al URI especificado en el objecto ApiRuta como una operación asincrónica.
		/// </summary>
		/// <typeparam name="T">Tipo</typeparam>
		/// <typeparam name="E">Elemento</typeparam>
		/// <param name="apiRuta">Objecto que contiene el API Endpoint</param>
		/// <param name="elemento">Parmetros en el query string de el endpoint, antes del ?</param>
		/// <returns>string con el resultado del request</returns>
		Task<IResponse<T>> PostAsync<T, E>(IApiEndpoint apiRuta, E elemento);
	}
}
