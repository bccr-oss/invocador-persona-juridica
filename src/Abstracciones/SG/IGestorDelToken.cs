using System.Threading.Tasks;
using Abstracciones.Contenedores;

namespace Abstracciones
{
	public interface IGestorDelToken
	{
		Task<Token> ObtieneElTokenAsync();
	}
}