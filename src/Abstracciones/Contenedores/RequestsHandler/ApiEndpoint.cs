using System;

namespace Abstracciones.Contenedores
{
	/// <summary>
	/// Representa el endpoint de un API.
	/// </summary>
	public class ApiEndpoint : IApiEndpoint, ICloneable
	{
		private int timeOut;
		private int timeOutEnSegundos;

		/// <summary>
		/// Constructor que inicializa los valores de la clase
		/// </summary>
		public ApiEndpoint()
		{
			Nombre = "";
			Ruta = "";
			timeOut = 100000;
			Monitoreable = false;
		}

		/// <summary>
		/// Nombre del api
		/// </summary>
		public string Nombre { get; set; }

		/// <summary>
		/// Ruta (url) del api
		/// </summary>
		public string Ruta { get; set; }

		/// <summary>
		/// Tiempo para desconección del api
		/// </summary>
		public int TimeOutEnSegundos
		{
			get => timeOutEnSegundos;

			set
			{
				timeOutEnSegundos = value;
				timeOut = value * 1000; 
			}
		}

		/// <summary>
		/// TimeOut para el HttpWebRequest. Es el valor en milisegundos.
		/// </summary>
		public int TimeOut => timeOut;

		/// <summary>
		/// Verificar si el api es monitoreable o no
		/// </summary>
		public bool Monitoreable { get; set; }


		/// <summary>
		/// Retorna una copia del objeto
		/// </summary>
		/// <returns></returns>
        public object Clone()
        {
			var apiEndpoint = new ApiEndpoint
			{
				Nombre = Nombre,
				Ruta = Ruta,
				Monitoreable = Monitoreable,
				TimeOutEnSegundos = TimeOutEnSegundos
			};

			return apiEndpoint;
        }

        /// <summary>
        /// Nombre del endpoint
        /// </summary>
        /// <returns></returns>
        public override string ToString()
		{
			return $"{Nombre}";
		}
	}
}
