using Microsoft.AspNetCore.Mvc;

namespace Abstracciones
{
    /// <summary>
    /// Abstracción para definir el proveedor de la información del suscriptor.
    /// </summary>
    public interface IProveedorDelSuscriptor
    {
        /// <summary>
        /// Nombre del suscriptor.
        /// </summary>
        /// <returns></returns>
        string GetSuscriptor();

        /// <summary>
        /// Versión solicitada.
        /// </summary>
        /// <returns></returns>
        ApiVersion GetVersion();
    }
}
