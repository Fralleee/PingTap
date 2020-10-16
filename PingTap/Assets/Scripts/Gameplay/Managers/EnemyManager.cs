using Fralle.AI;
using Fralle.Core.Attributes;
using Fralle.Core.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Gameplay
{
	public class EnemyManager : MonoBehaviour
	{
		public List<SpawnWave> waves;

		[Space(10)]
		[Readonly] public int enemiesSpawned;
		[Readonly] public int enemiesKilled;
		[Readonly] public int totalEnemies;

		[HideInInspector] public Spawner spawner;

		bool spawnComplete = false;
		bool allEnemiesDead => spawnComplete && enemiesSpawned - enemiesKilled == 0;
		public bool AllEnemiesDead;

		void Awake()
		{
			spawner = GetComponent<Spawner>();
			spawner.OnSpawnComplete += HandleSpawnComplete;
			Enemy.OnAnyEnemyDeath += HandleEnemyDeath;
		}

		public void PrepareSpawner()
		{
			var wave = waves.PopAt(0);
			spawner.SetSpawnDefinition(wave);
		}

		public void StartSpawner()
		{
			spawner.StartSpawning();
			spawnComplete = false;
			enemiesSpawned = 0;
			enemiesKilled = 0;
		}

		void HandleSpawnComplete(int enemyCount)
		{
			enemiesSpawned = enemyCount;
			spawnComplete = true;
		}

		void HandleEnemyDeath(Enemy enemy)
		{
			enemiesKilled += 1;
			if (allEnemiesDead)
			{
				AllEnemiesDead = true;
				EventManager.Broadcast(new GameOverEvent(true));
			}
		}
	}
}
