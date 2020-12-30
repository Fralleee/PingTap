using UnityEngine;

namespace Fralle.Core.Extensions
{
	public static class ColorExtensions
	{
		public static Color With(this Color c, float? r = null, float? g = null, float? b = null, float? a = null)
		{
			return new Color(r ?? c.r, g ?? c.g, b ?? c.b, a ?? c.a);
		}

		public static Color Alpha(this Color c, float alpha)
		{
			return new Color(c.r, c.g, c.b, alpha);
		}
	}
}
