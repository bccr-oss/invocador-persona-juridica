namespace Abstracciones
{
	/// <summary>
	/// 
	/// </summary>
	public interface IProveedorApiEndpoints
    {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="elNombre"></param>
		/// <returns></returns>
        string ObtengaLaRutaDelApi(string elNombre);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="elNombre"></param>
		/// <returns></returns>
		int ObtengaElTimeOutDelRequest(string elNombre);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="elNombre"></param>
		/// <returns></returns>
		IApiEndpoint ObtengaLaConfiguracion(string elNombre);
    }
}
