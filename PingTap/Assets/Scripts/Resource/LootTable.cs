using NaughtyAttributes;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fralle.Resource
{
  [CreateAssetMenu(menuName = "Loot/Setup")]
  public class LootTable : ScriptableObject
  {
    [Header("Poor")]
    [SerializeField] GameObject poorPrefab;
    [SerializeField] [MinMaxSlider(0, 1000)] Vector2 poorCreditsDropped;

    [Header("Common")]
    [SerializeField] GameObject commonPrefab;
    [SerializeField] [MinMaxSlider(0, 1000)] Vector2 commonCreditsDropped;

    [Header("Uncommon")]
    [SerializeField] GameObject uncommonPrefab;
    [SerializeField] [MinMaxSlider(0, 1000)] Vector2 uncommonCreditsDropped;

    [Header("Rare")]
    [SerializeField] GameObject rarePrefab;
    [SerializeField] [MinMaxSlider(0, 1000)] Vector2 rareCreditsDropped;

    [Header("Epic")]
    [SerializeField] GameObject epicPrefab;
    [SerializeField] [MinMaxSlider(0, 1000)] Vector2 epicCreditsDropped;

    [Header("Legendary")]
    [SerializeField] GameObject legendaryPrefab;
    [SerializeField] [MinMaxSlider(0, 1000)] Vector2 legendaryCreditsDropped;

    static int GetRandomRange(Vector2 range) => (int)Math.Round(Random.Range(range.x, range.y));

    public GameObject GetQualityPrefab(LootQuality.Type quality)
    {
      switch (quality)
      {
        case LootQuality.Type.Poor: return poorPrefab;
        case LootQuality.Type.Common: return commonPrefab;
        case LootQuality.Type.Uncommon: return uncommonPrefab;
        case LootQuality.Type.Rare: return rarePrefab;
        case LootQuality.Type.Epic: return epicPrefab;
        case LootQuality.Type.Legendary: return legendaryPrefab;
        default: return poorPrefab;
      };
    }
    public int GetQualityCredits(LootQuality.Type quality)
    {
      switch (quality)
      {
        case LootQuality.Type.Poor: return GetRandomRange(poorCreditsDropped);
        case LootQuality.Type.Common: return GetRandomRange(commonCreditsDropped);
        case LootQuality.Type.Uncommon: return GetRandomRange(uncommonCreditsDropped);
        case LootQuality.Type.Rare: return GetRandomRange(rareCreditsDropped);
        case LootQuality.Type.Epic: return GetRandomRange(epicCreditsDropped);
        case LootQuality.Type.Legendary: return GetRandomRange(legendaryCreditsDropped);
        default: return GetRandomRange(poorCreditsDropped);
      };
    }

    public void DropLoot(Vector3 position)
    {
      var quality = LootQuality.RandomQuality();
      var credits = GetQualityCredits(quality);
      var prefab = GetQualityPrefab(quality);

      var instance = Instantiate(prefab, position, Quaternion.identity);
      instance.GetComponent<Loot>().Setup(credits);
    }
  }
}