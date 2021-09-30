using System;
using System.Threading;
using System.Threading.Tasks;
using Abstracciones;
using Abstracciones.SG;

namespace SG.ProveedorJwt
{
    public class ProveedorJsonWebToken : IProveedorJsonWebToken
    {
        private readonly IGestorDelToken _gestorDelToken;
        private readonly IGestorDeCache _gestorDeCache;
        private readonly IProveedorDelSuscriptor _proveedorDelSuscriptor;
        private readonly ManualResetEvent _mreObteniendoToken;

        public ProveedorJsonWebToken(IGestorDelToken gestorDelToken, IGestorDeCache gestorDeCache, IProveedorDelSuscriptor proveedorDelSuscriptor)
        {
            _mreObteniendoToken = new ManualResetEvent(true);
            _gestorDelToken = gestorDelToken;
            _gestorDeCache = gestorDeCache;
            _proveedorDelSuscriptor = proveedorDelSuscriptor;
        }

        // Buscar en caché
        // Si no está en la caché, o está vencido pedir uno nuevo
        // Obtener el token desde el autorizador
        // Guardar el Token
        // Retornar el Access_Token

        public async Task<string> ProveaElTokenDeAcceso()
        {
            _mreObteniendoToken.WaitOne();
            var elToken = await _gestorDeCache.RetornaValorDesdeCache($"{_proveedorDelSuscriptor.GetSuscriptor()}_Token");

            if ((elToken == null) || (elToken.Expires_Time.Subtract(DateTime.Now).TotalSeconds <= 0))
            {
                _mreObteniendoToken.Reset();
                elToken = await _gestorDelToken.ObtieneElTokenAsync();
                _mreObteniendoToken.Set();
            }
            else
                await _gestorDeCache.RefrescarValorEnCache($"{_proveedorDelSuscriptor.GetSuscriptor()}_Token");

            return elToken.access_token;
        }
    }
}
