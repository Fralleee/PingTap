using Fralle.AI;
using Fralle.Core.Extensions;
using Fralle.Gameplay;
using UnityEngine;
using UnityEngine.AI;

public class Spawner2 : MonoBehaviour
{
  [SerializeField] GameObject enemyPrefab = null;

  [SerializeField] int enemiesToSpawn = 20;

  [SerializeField] float spawnRate = 1f;
  [SerializeField] float minRange = 10f;
  [SerializeField] float maxRange = 10f;

  PlayerHome playerHome;
  new Transform transform;
  float nextSpawnTime;
  int spawnedEnemies;

  void Awake()
  {
    transform = GetComponent<Transform>();
    playerHome = FindObjectOfType<PlayerHome>();
  }

  void Update()
  {
    if (spawnedEnemies < enemiesToSpawn && nextSpawnTime <= Time.time)
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
