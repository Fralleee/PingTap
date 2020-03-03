using UnityEngine;

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
    Vector3 targetPostition = new Vector3(target.position.x, source.position.y, target.position.z);
    source.LookAt(targetPostition);
  }

}
