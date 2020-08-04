using Fralle.AI;
using Fralle.Core.Extensions;
using Fralle.Gameplay;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
  public Vector3 spawnCenter = Vector3.zero;
  public float minRange = 10f;
  public float maxRange = 10f;

  SpawnWave currentWave;
  PlayerHome playerHome;
  new Transform transform;

  bool allowedSpawn;
  bool ShouldSpawnEnemy => allowedSpawn && currentSpawnCount < maxSpawnCount && nextSpawnTime <= Time.time;
  float nextSpawnTime;

  int maxSpawnCount;
  int currentSpawnCount;

  public void SetSpawnDefinition(SpawnWave wave)
  {
    allowedSpawn = false;
    maxSpawnCount = wave.count;
    currentWave = Instantiate(wave);
    currentWave.SetupProbabilityList();

    currentSpawnCount = 0;
    nextSpawnTime = Time.time + currentWave.spawnRate;
    Debug.Log($"Should spawn enemy every {currentWave.spawnRate}s");
  }

  public void StartSpawning()
  {
    allowedSpawn = true;
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
      var timeDiff = Time.time - nextSpawnTime;
      var spawnCount = Mathf.Floor(timeDiff / currentWave.spawnRate);
      for (int i = 0; i < spawnCount; i++)
      {
        Spawn();
      }
    }
  }

  void Spawn()
  {
    GameObject prefab = currentWave.GetPrefab();
    Vector3 position = GetSpawnPoint();
    if (position == Vector3.zero) return;

    var spawnedInstance = Instantiate(prefab, position, Quaternion.identity);
    spawnedInstance.GetComponent<AINavigation>().target = playerHome.transform;

    nextSpawnTime += currentWave.spawnRate;
    currentSpawnCount += 1;
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
