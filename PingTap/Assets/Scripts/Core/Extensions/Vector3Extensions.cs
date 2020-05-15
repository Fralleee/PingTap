using UnityEngine;

namespace Fralle.Core.Extensions
{
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

    public static void RoundUp(this Vector3 v)
    {
      v.x = Mathf.Round(v.x + .5f);
      v.y = Mathf.Round(v.y + .5f);
      v.z = Mathf.Round(v.z + .5f);
    }

    public static void RoundDown(this Vector3 v)
    {
      v.x = Mathf.Round(v.x - .5f);
      v.y = Mathf.Round(v.y - .5f);
      v.z = Mathf.Round(v.z - .5f);
    }

    public static void Round(this Vector3 v)
    {
      v.x = Mathf.Round(v.x);
      v.y = Mathf.Round(v.y);
      v.z = Mathf.Round(v.z);
    }

    public static void Floor(this Vector3 v)
    {
      v.x = Mathf.Floor(v.x);
      v.y = Mathf.Floor(v.y);
      v.z = Mathf.Floor(v.z);
    }

    //

    public static Vector3 RoundedUp(this Vector3 t)
    {
      Vector3 v = new Vector3();
      v.x = Mathf.Round(t.x + .5f);
      v.y = Mathf.Round(t.y + .5f);
      v.z = Mathf.Round(t.z + .5f);
      return v;
    }

    public static Vector3 RoundedDown(this Vector3 t)
    {
      Vector3 v = new Vector3();
      v.x = Mathf.Round(t.x - .5f);
      v.y = Mathf.Round(t.y - .5f);
      v.z = Mathf.Round(t.z - .5f);
      return v;
    }

    public static Vector3 Rounded(this Vector3 t)
    {
      Vector3 v = new Vector3();
      v.x = Mathf.Round(t.x);
      v.y = Mathf.Round(t.y);
      v.z = Mathf.Round(t.z);
      return v;
    }

    public static Vector3 Floored(this Vector3 t)
    {
      Vector3 v = new Vector3();
      v.x = Mathf.Floor(t.x);
      v.y = Mathf.Floor(t.y);
      v.z = Mathf.Floor(t.z);
      return v;
    }
  }
}