using Fralle.AI;
using Fralle.Core.Extensions;
using Fralle.Gameplay;
using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
  public static event Action OnNewWave = delegate { };
  public static event Action OnWavesComplete = delegate { };

  public Vector3 spawnCenter = Vector3.zero;
  public GameObject enemyPrefab = null;
  public int enemiesToSpawn = 20;
  public float spawnRate = 1f;
  public float minRange = 10f;
  public float maxRange = 10f;

  PlayerHome playerHome;
  new Transform transform;

  bool ShouldSpawnEnemy => spawnedEnemies < enemiesToSpawn && nextSpawnTime <= Time.time;
  float nextSpawnTime;
  int spawnedEnemies;

  public void Reset()
  {
    spawnedEnemies = 0;
    nextSpawnTime = 0;
  }

  void Awake()
  {
    transform = GetComponent<Transform>();
    playerHome = FindObjectOfType<PlayerHome>();
  }

  void Update()
  {
    if (ShouldSpawnEnemy)
    {
      Spawn();
    }
  }

  void Spawn()
  {
    Vector3 position = GetSpawnPoint();
    if (position == Vector3.zero) return;

    var enemyInstance = Instantiate(enemyPrefab, position, Quaternion.identity);
    var enemy = enemyInstance.GetComponent<Enemy>();
    ((EnemyTargetNavigation)enemy.enemyNavigation).target = playerHome.transform;

    nextSpawnTime = Time.time + spawnRate;
    spawnedEnemies += 1;
  }

  Vector3 GetSpawnPoint()
  {
    Vector3 randomVector = Random.insideUnitSphere.With(y: 0).normalized * Random.Range(minRange, maxRange);
    for (int i = 0; i < 30; i++)
    {
      if (NavMesh.SamplePosition(transform.position + randomVector, out NavMeshHit hit, maxRange, NavMesh.AllAreas))
      {
        return hit.position;
      }
    }

    return Vector3.zero;
  }
}
