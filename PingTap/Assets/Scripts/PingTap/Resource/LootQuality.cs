using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fralle.Resource
{
  public static class LootQuality
  {
    public enum Type
    {
      Poor,
      Common,
      Uncommon,
      Rare,
      Epic,
      Legendary
    }

    static readonly Dictionary<Type, float> QualityChance = new Dictionary<Type, float>
    {
      {Type.Legendary, 0.001f},  // 0.1%
      {Type.Epic, 0.01f},        // 1%
      {Type.Rare, 0.05f},        // 5%
      {Type.Uncommon, 0.125f},   // 12.5%
      {Type.Common, 0.3f},       // 30%
      {Type.Poor, 1}             // > 100%
    };
    static readonly Dictionary<Type, Color> QualityColorDict = new Dictionary<Type, Color>
    {
      {Type.Poor, Color.gray},
      {Type.Common, Color.white},
      {Type.Uncommon, Color.green},
      {Type.Rare, Color.blue},
      {Type.Epic, Color.magenta},
      {Type.Legendary, Color.yellow}
    };
    public static Color GetQualityColor(Type quality) => QualityColorDict[quality];
    public static Type RandomQuality()
    {
      float value = Random.value;
      foreach (KeyValuePair<Type, float> chance in QualityChance)
      {
        if (value < chance.Value) return chance.Key;
      }
      return Type.Poor;
    }
  }
}
