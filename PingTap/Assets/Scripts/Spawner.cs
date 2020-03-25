using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
  [SerializeField] Enemy enemyPrefab;
  [SerializeField] int count = 30;

  readonly List<Enemy> spawnedEnemies = new List<Enemy>();
  GameObject enemiesParentObject;

  void Start()
  {
    enemiesParentObject = GameObject.Find("Enemies");
    if (enemiesParentObject == null) new GameObject("Enemies");
    Spawn();
  }

  void Spawn()
  {
    for (int i = 0; i < count; i++)
    {
      var enemy = Instantiate(enemyPrefab, enemiesParentObject.transform);
      spawnedEnemies.Add(enemy);
      enemy.OnDeath += x => spawnedEnemies.Remove(enemy);
    }
  }
}
