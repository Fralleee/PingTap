using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
[CreateAssetMenu(menuName = "Spawn/Wave")]
public class SpawnWave : ScriptableObject
{
  public List<SpawnProbability> spawnProbabilities = new List<SpawnProbability>();
  public int count;
  public float spawnOverTimeInSeconds;
  [HideInInspector] public float spawnRate;

  public void SetupProbabilityList()
  {
    spawnRate = spawnOverTimeInSeconds / count;

    var chanceSum = spawnProbabilities.Sum(x => x.chance);
    spawnProbabilities.ForEach(x => x.chance /= chanceSum);
    spawnProbabilities.OrderBy(x => x.chance);

    float oldValue = 0;
    foreach (var spawnProbability in spawnProbabilities)
    {
      spawnProbability.chance += oldValue;
      oldValue = spawnProbability.chance;
    }
  }

  public GameObject GetPrefab()
  {
    var random = Random.Range(0f, 1f);
    var probability = spawnProbabilities.Where(x => random <= x.chance).FirstOrDefault();

    return probability.prefab;
  }
}
