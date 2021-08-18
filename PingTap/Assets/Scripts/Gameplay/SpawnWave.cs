using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fralle.Gameplay
{
  [Serializable]
  [CreateAssetMenu(menuName = "Spawn/Wave")]
  public class SpawnWave : ScriptableObject
  {
    public List<SpawnProbability> SpawnProbabilities = new List<SpawnProbability>();
    public int Count;
    public float SpawnOverTimeInSeconds;
    [HideInInspector] public float SpawnRate;

    public void SetupProbabilityList()
    {
      SpawnRate = SpawnOverTimeInSeconds / Count;

      float chanceSum = SpawnProbabilities.Sum(x => x.Chance);
      SpawnProbabilities.ForEach(x => x.Chance /= chanceSum);

      float oldValue = 0;
      foreach (SpawnProbability spawnProbability in SpawnProbabilities.OrderBy(x => x.Chance))
      {
        spawnProbability.Chance += oldValue;
        oldValue = spawnProbability.Chance;
      }
    }

    public GameObject GetPrefab()
    {
      float random = Random.Range(0f, 1f);
      SpawnProbability probability = SpawnProbabilities.FirstOrDefault(x => random <= x.Chance);

      return probability.Prefab;
    }
  }
}
