using UnityEngine;

public static class Vector3Extensions
{
  public static Vector3 SnapToGrid(this Vector3 vector, float gridSize)
  {
    return new Vector3(
      Mathf.Round(vector.x / gridSize) * gridSize,
      Mathf.Round(vector.y / gridSize) * gridSize,
      Mathf.Round(vector.z / gridSize) * gridSize
    );
  }

  public static Vector3 With(this Vector3 v, float? x = null, float? y = null, float? z = null)
  {
    return new Vector3(x ?? v.x, y ?? v.y, z ?? v.z);
  }

  public static Vector3 Flat(this Vector3 v)
  {
    return new Vector3(v.x, 0, v.z);
  }

  public static Vector3 DirectionTo(this Vector3 source, Vector3 destination)
  {
    return Vector3.Normalize(destination - source);
  }

  public static bool InViewPort(this Vector3 v)
  {
    return v.x >= 0 && v.x <= 1 && v.y >= 0 && v.y <= 1 && v.z >= 0;
  }

}