using System.Threading.Tasks;

namespace Abstracciones
{

    /// <summary>
    /// Abstracción para proveer del token de autenticación.
    /// </summary>
    public interface IProveedorJsonWebToken
    {
        /// <summary>
        /// Retorna el Access Token.
        /// </summary>
        /// <returns>Jwt</returns>
        Task<string> ProveaElTokenDeAcceso();
    }
}