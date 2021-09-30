using Abstracciones;
using Abstracciones.SG;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SG.GestorDeCache;
using SG.ProveedorJwt;
using SG.RenovarToken;

namespace InvocadorPersonaJuridica.Api
{
    internal static class ConfiguracionDelVersionamiento
	{
		internal static IServiceCollection AgregarElVersionamiento(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<IProveedorDelSuscriptor, ProveedorDelSuscriptor>();
			services.AddSingleton<IProveedorApiEndpoints, ProveedorApiEndpoints>();
			services.AddScoped<IRequestsHandler, RequestsHandler>();
			services.AddScoped<IGestorDeCache, ConsultarCache>();
			services.AddScoped<IGestorDelToken, GestorDelToken>();
			services.AddScoped<IProveedorJsonWebToken, ProveedorJsonWebToken>();

			var options = configuration.GetSection(nameof(DistributedCacheEntryOptions)).Get<DistributedCacheEntryOptions>();
			services.AddSingleton(options);

			services.AddDistributedRedisCache(options =>
			{
				options.Configuration = configuration.GetConnectionString("Redis");
			});

			services.AddApiVersioning(opciones =>
					{
						opciones.ReportApiVersions = true;
						opciones.DefaultApiVersion = new ApiVersion(1, 0);
						opciones.AssumeDefaultVersionWhenUnspecified = true;
						opciones.UseApiBehavior = false;
						opciones.ApiVersionReader = new QueryStringApiVersionReader("api-version");
					});

			return services;
		}
	}
}
