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

		public Vector3 SpawnCenter = Vector3.zero;
		public float MinRange = 10f;
		public float MaxRange = 10f;

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
			maxSpawnCount = wave.Count;
			currentWave = Instantiate(wave);
			currentWave.SetupProbabilityList();

			currentSpawnCount = 0;
			enemyCount = 0;
			nextSpawnTime = Time.time + currentWave.SpawnRate;
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
			if (!ShouldSpawnEnemy)
				return;
			float timeDiff = Time.time - nextSpawnTime;
			float spawnCount = Mathf.Floor(timeDiff / currentWave.SpawnRate);
			for (int i = 0; i < spawnCount; i++)
			{
				Spawn();
			}
		}

		void Spawn()
		{
			GameObject prefab = currentWave.GetPrefab();
			Vector3 position = GetSpawnPoint();
			if (position == Vector3.zero)
				return;

			GameObject spawnedInstance = Instantiate(prefab, position.With(y: 0), Quaternion.identity);
			spawnedInstance.name = prefab.name;

			//spawnedInstance.GetComponent<AiNavigation>().SetDestination(playerHome.Entry);

			if (spawnedInstance.GetComponent<ScoreController>())
				enemyCount += 1;

			nextSpawnTime += currentWave.SpawnRate;
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

		//public void SpawnEnemyType(EnemyType enemyType, int count)
		//{
		//	for (int i = 0; i < count; i++)
		//	{
		//		PerformSpawn(EnemyAtlas.Instance.GetPrefab(enemyType));
		//	}
		//}

		public void PerformSpawn(GameObject enemy)
		{
			Vector3 position = GetSpawnPoint();
			if (position == Vector3.zero)
			{
				Debug.LogError("GetSpawnPoint returned Vector3.zero. THIS NEEDS TO BE FIXED!");
				return;
			}

			GameObject spawnedInstance = Instantiate(enemy, position.With(y: 0), Quaternion.identity);
			spawnedInstance.name = enemy.name;
			//spawnedInstance.GetComponent<AiNavigation>().SetDestination(playerHome.Entry);
			if (spawnedInstance.GetComponent<ScoreController>())
				enemyCount += 1;
		}

		Vector3 GetSpawnPoint()
		{
			for (int i = 0; i < 30; i++)
			{
				Vector3 randomVector = Random.insideUnitSphere.With(y: 0).normalized * Random.Range(MinRange, MaxRange);
				if (NavMesh.SamplePosition((SpawnCenter + randomVector).With(y: 0), out NavMeshHit hit, 2f, NavMesh.AllAreas))
				{
					return hit.position;
				}
			}

			return Vector3.zero;
		}
	}
}
