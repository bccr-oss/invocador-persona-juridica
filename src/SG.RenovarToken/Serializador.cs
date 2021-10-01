using System;
using System.Linq;
using System.Web;

namespace SG.RenovarToken
{
	internal static class Serializador
	{
		internal static string Serialice(object objeto)
		{
			string laRespuesta = string.Empty;

			if (objeto != null)
			{
				var properties = from p in objeto.GetType().GetProperties()
								 where p.GetValue(objeto, null) != null
								 select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(objeto).ToString());

				laRespuesta = String.Join("&", properties.ToArray());
			}

			return laRespuesta;
		}
	}
}
