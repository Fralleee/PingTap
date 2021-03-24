using UnityEngine;

namespace Fralle.Core.Extensions
{
	public static class Vector2Extensions
	{
		public static Vector3 ToVector3(this Vector2 vector) => new Vector3(vector.x, 0, vector.y);
	}
}
