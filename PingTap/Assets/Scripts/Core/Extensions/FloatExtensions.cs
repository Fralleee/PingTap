using System;
using UnityEngine;

namespace Fralle.Core.Extensions
{
  public static class FloatExtensions
  {
    public static float Floor(this float value)
    {
      return Mathf.Floor(value);
    }

    public static float Round(this float value)
    {
      return Mathf.Round(value);
    }

    public static float Ceil(this float value)
    {
      return Mathf.Ceil(value);
    }

    public static bool EqualsWithTolerance(this float value, float otherValue, float tolerance = 0.000001f)
    {
      return Math.Abs(value - otherValue) < tolerance;
    }
  }
}