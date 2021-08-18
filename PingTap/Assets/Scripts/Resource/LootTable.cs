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
    [SerializeField] Vector2 poorCreditsDropped = Vector2.zero;

    [Header("Common")]
    [SerializeField] GameObject commonPrefab = null;
    [SerializeField] Vector2 commonCreditsDropped = Vector2.zero;

    [Header("Uncommon")]
    [SerializeField] GameObject uncommonPrefab = null;
    [SerializeField] Vector2 uncommonCreditsDropped = Vector2.zero;

    [Header("Rare")]
    [SerializeField] GameObject rarePrefab = null;
    [SerializeField] Vector2 rareCreditsDropped = Vector2.zero;

    [Header("Epic")]
    [SerializeField] GameObject epicPrefab = null;
    [SerializeField] Vector2 epicCreditsDropped = Vector2.zero;

    [Header("Legendary")]
    [SerializeField] GameObject legendaryPrefab = null;
    [SerializeField] Vector2 legendaryCreditsDropped = Vector2.zero;

    static int GetRandomRange(Vector2 range) => (int)Math.Round(Random.Range(range.x, range.y));

    public GameObject GetQualityPrefab(LootQuality.Type quality)
    {
      return quality switch
      {
        LootQuality.Type.Poor => poorPrefab,
        LootQuality.Type.Common => commonPrefab,
        LootQuality.Type.Uncommon => uncommonPrefab,
        LootQuality.Type.Rare => rarePrefab,
        LootQuality.Type.Epic => epicPrefab,
        LootQuality.Type.Legendary => legendaryPrefab,
        _ => poorPrefab
      };
    }
    public int GetQualityCredits(LootQuality.Type quality)
    {
      return quality switch
      {
        LootQuality.Type.Poor => GetRandomRange(poorCreditsDropped),
        LootQuality.Type.Common => GetRandomRange(commonCreditsDropped),
        LootQuality.Type.Uncommon => GetRandomRange(uncommonCreditsDropped),
        LootQuality.Type.Rare => GetRandomRange(rareCreditsDropped),
        LootQuality.Type.Epic => GetRandomRange(epicCreditsDropped),
        LootQuality.Type.Legendary => GetRandomRange(legendaryCreditsDropped),
        _ => GetRandomRange(poorCreditsDropped)
      };
    }

    public void DropLoot(Vector3 position)
    {
      LootQuality.Type quality = LootQuality.RandomQuality();
      int credits = GetQualityCredits(quality);
      GameObject prefab = GetQualityPrefab(quality);

      GameObject instance = Instantiate(prefab, position, Quaternion.identity);
      instance.GetComponent<Loot>().Setup(credits);
    }
  }
}
