using System.Threading.Tasks;
using Abstracciones.Contenedores;

namespace Abstracciones.SG
{
	public interface IGestorDeCache
	{
		Task AlmacenaValorEnCache(string key, Token registro);

		Task<Token> RetornaValorDesdeCache(string key);

		Task RefrescarValorEnCache(string key);
	}
}
