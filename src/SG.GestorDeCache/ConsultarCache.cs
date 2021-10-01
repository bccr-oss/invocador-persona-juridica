using System.Threading.Tasks;
using Abstracciones.Contenedores;
using Abstracciones.SG;
using Microsoft.Extensions.Caching.Distributed;

namespace SG.GestorDeCache
{
    public class ConsultarCache : IGestorDeCache
	{
		private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _options;

        public ConsultarCache(IDistributedCache cache, DistributedCacheEntryOptions options)
		{
			_cache = cache;
			_options = options;
		}

		public async Task AlmacenaValorEnCache(string key, Token registro)
		{
			await _cache.SetAsync(key, registro.Serializado(), _options);
		}

		public async Task<Token> RetornaValorDesdeCache(string key)
		{
			var resultado = await _cache.GetAsync(key);
			var token = CacheExtensiones.Deserializer<Token>(resultado);

			return token;
		}

		public async Task RefrescarValorEnCache(string key)
		{
			await _cache.RefreshAsync(key);
		}
	}
}
