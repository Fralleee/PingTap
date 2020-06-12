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
    [SerializeField] GameObject poorPrefab = null;
    [SerializeField] [MinMaxSlider(0, 1000)] Vector2 poorCreditsDropped = Vector2.zero;

    [Header("Common")]
    [SerializeField] GameObject commonPrefab = null;
    [SerializeField] [MinMaxSlider(0, 1000)] Vector2 commonCreditsDropped = Vector2.zero;

    [Header("Uncommon")]
    [SerializeField] GameObject uncommonPrefab = null;
    [SerializeField] [MinMaxSlider(0, 1000)] Vector2 uncommonCreditsDropped = Vector2.zero;

    [Header("Rare")]
    [SerializeField] GameObject rarePrefab = null;
    [SerializeField] [MinMaxSlider(0, 1000)] Vector2 rareCreditsDropped = Vector2.zero;

    [Header("Epic")]
    [SerializeField] GameObject epicPrefab = null;
    [SerializeField] [MinMaxSlider(0, 1000)] Vector2 epicCreditsDropped = Vector2.zero;

    [Header("Legendary")]
    [SerializeField] GameObject legendaryPrefab = null;
    [SerializeField] [MinMaxSlider(0, 1000)] Vector2 legendaryCreditsDropped = Vector2.zero;

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