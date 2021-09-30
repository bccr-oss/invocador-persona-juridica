
namespace Abstracciones.Contenedores
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Response<T> : IResponse<T>
	{
		/// <summary>
		/// 
		/// </summary>
		public bool Succeeded { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Error { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public T Respuesta { get; set; }
	}
}
