using Fralle.AI;
using Fralle.Core.Extensions;
using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Fralle.Gameplay
{
	public class Spawner : MonoBehaviour
	{
		public event Action<int> OnSpawnComplete = delegate { };

		[SerializeField] GameObject enemyPrefab;

		public Vector3 spawnCenter = Vector3.zero;
		public float minRange = 10f;
		public float maxRange = 10f;

		SpawnWave currentWave;
		HeadQuarters playerHome;

		bool allowedSpawn;
		bool ShouldSpawnEnemy => allowedSpawn && currentSpawnCount < maxSpawnCount && nextSpawnTime <= Time.time;
		float nextSpawnTime;

		int maxSpawnCount;
		int currentSpawnCount;
		int enemyCount;

		public void SetSpawnDefinition(SpawnWave wave)
		{
			allowedSpawn = false;
			maxSpawnCount = wave.count;
			currentWave = Instantiate(wave);
			currentWave.SetupProbabilityList();

			currentSpawnCount = 0;
			enemyCount = 0;
			nextSpawnTime = Time.time + currentWave.spawnRate;
		}

		public void StartSpawning()
		{
			allowedSpawn = true;
		}

		void Awake()
		{
			playerHome = FindObjectOfType<HeadQuarters>();
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
			if (position == Vector3.zero)
				return;

			var spawnedInstance = Instantiate(prefab, position.With(y: 0), Quaternion.identity);
			spawnedInstance.name = prefab.name;

			spawnedInstance.GetComponent<AINavigation>().SetDestination(playerHome.Entry);

			if (spawnedInstance.GetComponent<Enemy>())
				enemyCount += 1;

			nextSpawnTime += currentWave.spawnRate;
			currentSpawnCount += 1;

			if (currentSpawnCount == maxSpawnCount)
				OnSpawnComplete(enemyCount);
		}

		public void SpawnEnemy(int count)
		{
			for (int i = 0; i < count; i++)
			{
				PerformSpawn(enemyPrefab);
			}
		}

		public void PerformSpawn(GameObject enemy)
		{
			Vector3 position = GetSpawnPoint();
			if (position == Vector3.zero)
			{
				Debug.LogError("GetSpawnPoint returned Vector3.zero. THIS NEEDS TO BE FIXED!");
				return;
			}

			var spawnedInstance = Instantiate(enemy, position.With(y: 0), Quaternion.identity);
			spawnedInstance.name = enemy.name;
			spawnedInstance.GetComponent<AINavigation>().SetDestination(playerHome.Entry);
			if (spawnedInstance.GetComponent<Enemy>())
				enemyCount += 1;
		}

		Vector3 GetSpawnPoint()
		{
			for (int i = 0; i < 30; i++)
			{
				Vector3 randomVector = Random.insideUnitSphere.With(y: 0).normalized * Random.Range(minRange, maxRange);
				if (NavMesh.SamplePosition((spawnCenter + randomVector).With(y: 0), out NavMeshHit hit, 2f, NavMesh.AllAreas))
				{
					return hit.position;
				}
			}

			return Vector3.zero;
		}
	}
}
