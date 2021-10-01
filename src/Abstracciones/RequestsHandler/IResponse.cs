namespace Abstracciones
{
	/// <summary>
	/// Encapsula el objeto de respuesta
	/// </summary>
	public interface IResponse<TOutput>
	{
		/// <summary>
		/// Gets or sets a value indicating whether the validation was successful.
		/// </summary>
		bool Succeeded { get; set; }

		/// <summary>
		/// Gets or sets the error.
		/// </summary>
		string Error { get; set; }

		/// <summary>
		/// Gets or sets the error description.
		/// </summary>
		string Description { get; set; }

		/// <summary>
		/// 
		/// </summary>
		TOutput Respuesta { get; set; }
	}
}
