using UnityEngine;

namespace Fralle.Core.Extensions
{
	public static class GameObjectExtensions
	{
		public static void SetLayerRecursively(this GameObject gameObject, int layer)
		{
			gameObject.layer = layer;
			foreach (Transform child in gameObject.transform)
			{
				SetLayerRecursively(child.gameObject, layer);
			}
		}

		public static void SetLayerRecursively(this GameObject gameObject, int layer, LayerMask layerMask)
		{
			if (layerMask.IsInLayerMask(gameObject.layer))
				return;

			gameObject.layer = layer;
			foreach (Transform child in gameObject.transform)
			{
				SetLayerRecursively(child.gameObject, layer, layerMask);
			}
		}
	}
}
