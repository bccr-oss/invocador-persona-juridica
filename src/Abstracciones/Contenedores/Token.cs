using System;

namespace Abstracciones.Contenedores
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class Token
	{
		public string refresh_token { get; set; }

		public string access_token { get; set; }

		public int expires_in { get; set; }

		public string token_type { get; set; }

		public string scope { get; set; }

		public DateTime Expires_Time { get; set; }
	}
}