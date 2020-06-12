using UnityEngine;

namespace Fralle.Core.Extensions
{
  public static class TransformExtensions
  {

    public static void EnableChildren(this Transform transform)
    {
      foreach (Transform t in transform) t.gameObject.SetActive(true);
    }

    public static void DisableChildren(this Transform transform)
    {
      foreach (Transform t in transform) t.gameObject.SetActive(false);
    }

    public static Vector3 DirectionTo(this Transform source, Transform destination)
    {
      return source.position.DirectionTo(destination.position);
    }

    public static void LookAtFlat(this Transform source, Transform target)
    {
      var position = new Vector3(target.position.x, source.position.y, target.position.z);
      source.LookAt(position);
    }

    public static Transform FindRecursively(this Transform aParent, string aName)
    {
      foreach (Transform child in aParent)
      {
        if (child.name == aName)
          return child;
        var result = child.FindRecursively(aName);
        if (result != null)
          return result;
      }
      return null;
    }
  }
}