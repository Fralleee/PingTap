using UnityEngine;

public static class GameObjectExtensions
{
  public static void SetLayerRecursively(this GameObject gameObject, int layer, int ignoreLayer = -1)
  {
    if (gameObject.layer == ignoreLayer) return;

    gameObject.layer = layer;
    foreach (Transform child in gameObject.transform)
    {
      SetLayerRecursively(child.gameObject, layer, ignoreLayer);
    }
  }

}
