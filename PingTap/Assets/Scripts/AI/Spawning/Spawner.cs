using Fralle.Core.Attributes;
using Fralle.Gameplay;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Fralle.AI.Spawning
{
  public class Spawner : MonoBehaviour
  {
    [SerializeField] float SpawnDelay = 0.1f;

    [Readonly] public Army army;

    PlayerHome playerHome;
    WaveDefinition waveDefinition;
    int waveCount;
    int spawnedCount;

    void Awake()
    {
      playerHome = FindObjectOfType<PlayerHome>();
    }

    public int SpawnRound(int round)
    {
      waveDefinition = army.NextWave(round);
      waveCount = waveDefinition.count;
      spawnedCount = 0;

      StartCoroutine(army.waypointSchemas.Length > 0 ? SpawnEnemiesWithWaypoints() : SpawnEnemies());

      return waveDefinition.count;
    }

    IEnumerator SpawnEnemies()
    {
      while (spawnedCount != waveCount)
      {
        var randomVector = Random.insideUnitCircle * 5f;
        var randomVector3 = new Vector3(randomVector.x, 0, randomVector.y);
        var enemy = Instantiate(waveDefinition.enemy, transform.position + randomVector3, Quaternion.identity, transform);
        ((EnemyTargetNavigation)enemy.enemyNavigation).target = playerHome.transform;
        spawnedCount++;
        yield return new WaitForSeconds(SpawnDelay);
      }
    }

    IEnumerator SpawnEnemiesWithWaypoints()
    {
      while (spawnedCount != waveCount)
      {
        var schema = army.waypointSchemas[spawnedCount % army.waypointSchemas.Length];
        var randomVector = Random.insideUnitCircle * 5f;
        var position = schema.waypoints.FirstOrDefault() + new Vector3(randomVector.x, 0f, randomVector.y);
        var enemy = Instantiate(waveDefinition.enemy, position, Quaternion.identity, transform);
        ((EnemyWaypointNavigation)enemy.enemyNavigation).wayPointSchema = schema;
        spawnedCount++;
        yield return new WaitForSeconds(SpawnDelay);
      }
    }
  }
}