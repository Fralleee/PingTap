using Fralle.Core.Attributes;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Fralle.AI.Spawning
{
  public class Spawner : MonoBehaviour
  {
    const float SpawnDelay = 0.2f;

    [Readonly] public Army army;

    WaveDefinition waveDefinition;
    int waveCount;
    int spawnedCount;

    public int SpawnRound(int round)
    {
      if (army.waypointSchemas.Length == 0) return 0;

      waveDefinition = army.NextWave(round);
      waveCount = waveDefinition.count;
      spawnedCount = 0;

      StartCoroutine(SpawnEnemies());

      return waveDefinition.count;
    }

    IEnumerator SpawnEnemies()
    {
      while (spawnedCount != waveCount)
      {
        var schema = army.waypointSchemas[spawnedCount % army.waypointSchemas.Length];
        var randomVector = Random.insideUnitCircle * 5f;
        var position = schema.waypoints.FirstOrDefault() + new Vector3(randomVector.x, 0f, randomVector.y);
        var enemy = Instantiate(waveDefinition.enemy, position, Quaternion.identity, transform);
        enemy.enemyNavigation.wayPointSchema = schema;
        spawnedCount++;
        yield return new WaitForSeconds(SpawnDelay);
      }
    }
  }
}