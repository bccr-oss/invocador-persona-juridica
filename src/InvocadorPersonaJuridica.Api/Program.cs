using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace InvocadorPersonaJuridica.Api
{
	public static class Program
	{
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Debug()
                .CreateBootstrapLogger();

            Log.Information("Starting up!");

            try
            {
                await BuildWebHost(args)
                    .RunAsync();

                Log.Information("Stopped cleanly");
                await Task.FromResult(0);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
                await Task.FromResult(1);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
			.UseIISIntegration()
			.UseStartup<Startup>()
			.Build();
	}
}