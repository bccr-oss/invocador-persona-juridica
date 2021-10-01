namespace Abstracciones
{
	/// <summary>
	/// Abstracción que representa un endpoint a invocar con el request handler.
	/// </summary>
	public interface IApiEndpoint
	{
		/// <summary>
		/// Nombre del Endpoint
		/// </summary>
		string Nombre { get; set; }

		/// <summary>
		/// Ruta (url) del api a invocar
		/// </summary>
		string Ruta { get; set; }

		/// <summary>
		/// Tiempo para desconección del api
		/// </summary>
		int TimeOutEnSegundos { get; set; }

		/// <summary>
		/// TimeOut para el HttpWebRequest. Es el valor en milisegundos.
		/// </summary>
		int TimeOut { get; }

		/// <summary>
		/// Verificar si el api es monitoreable o no
		/// </summary>
		bool Monitoreable { get; set; }
	}
}
