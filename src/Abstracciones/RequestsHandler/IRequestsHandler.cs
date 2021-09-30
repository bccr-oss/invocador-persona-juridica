namespace Abstracciones
{
	/// <summary>
	/// Abstracción para registrar en el IOC la concreción encargada de realizar los requests a las api's.
	/// </summary>
	public interface IRequestsHandler : IRequestsHandlerGet, IRequestsHandlerPost, IRequestsHandlerPut, IRequestsHandlerDelete
	{

	}
}
