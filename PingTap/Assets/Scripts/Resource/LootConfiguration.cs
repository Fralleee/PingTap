using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fralle.Resource
{
  [CreateAssetMenu(menuName = "Loot/Setup")]
  public class LootConfiguration : ScriptableObject
  {
    [Header("Effects")]
    [SerializeField] GameObject poorPrefab;
    [SerializeField] GameObject commonPrefab;
    [SerializeField] GameObject uncommonPrefab;
    [SerializeField] GameObject rarePrefab;
    [SerializeField] GameObject epicPrefab;
    [SerializeField] GameObject legendaryPrefab;

    [Header("Credits")]
    [SerializeField] [MinMaxSlider(0, 1000)] Vector2 poorCreditsDropped;
    [SerializeField] [MinMaxSlider(0, 1000)] Vector2 commonCreditsDropped;
    [SerializeField] [MinMaxSlider(0, 1000)] Vector2 uncommonCreditsDropped;
    [SerializeField] [MinMaxSlider(0, 1000)] Vector2 rareCreditsDropped;
    [SerializeField] [MinMaxSlider(0, 1000)] Vector2 epicCreditsDropped;
    [SerializeField] [MinMaxSlider(0, 1000)] Vector2 legendaryCreditsDropped;

    static readonly Dictionary<LootQuality, float> QualityChance = new Dictionary<LootQuality, float>
    {
      {LootQuality.Legendary, 0.001f},  // 0.1%
      {LootQuality.Epic, 0.01f},        // 1%
      {LootQuality.Rare, 0.05f},        // 5%
      {LootQuality.Uncommon, 0.125f},   // 12.5%
      {LootQuality.Common, 0.3f},       // 30%
      {LootQuality.Poor, 1}             // > 100%
    };
    static readonly Dictionary<LootQuality, Color> QualityColorDict = new Dictionary<LootQuality, Color>
    {
      {LootQuality.Poor, Color.gray},
      {LootQuality.Common, Color.white},
      {LootQuality.Uncommon, Color.green},
      {LootQuality.Rare, Color.blue},
      {LootQuality.Epic, Color.magenta},
      {LootQuality.Legendary, Color.yellow}
    };

    public Color GetQualityColor(LootQuality quality) => QualityColorDict[quality];
    public GameObject GetQualityPrefab(LootQuality quality)
    {
      switch (quality)
      {
        case LootQuality.Poor: return poorPrefab;
        case LootQuality.Common: return commonPrefab;
        case LootQuality.Uncommon: return uncommonPrefab;
        case LootQuality.Rare: return rarePrefab;
        case LootQuality.Epic: return epicPrefab;
        case LootQuality.Legendary: return legendaryPrefab;
        default: return poorPrefab;
      };
    }

    public int GetQualityCredits(LootQuality quality)
    {
      switch (quality)
      {
        case LootQuality.Poor: return GetRandomRange(poorCreditsDropped);
        case LootQuality.Common: return GetRandomRange(commonCreditsDropped);
        case LootQuality.Uncommon: return GetRandomRange(uncommonCreditsDropped);
        case LootQuality.Rare: return GetRandomRange(rareCreditsDropped);
        case LootQuality.Epic: return GetRandomRange(epicCreditsDropped);
        case LootQuality.Legendary: return GetRandomRange(legendaryCreditsDropped);
        default: return GetRandomRange(poorCreditsDropped);
      };
    }

    static int GetRandomRange(Vector2 range) => (int)Math.Round(Random.Range(range.x, range.y));

    public LootQuality GetQuality()
    {
      var value = Random.value;
      foreach (var chance in QualityChance)
      {
        if (value < chance.Value) return chance.Key;
      }
      return LootQuality.Poor;
    }
  }
}