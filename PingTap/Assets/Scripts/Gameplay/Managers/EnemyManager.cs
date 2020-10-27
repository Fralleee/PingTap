using Fralle.AI;
using Fralle.Core.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Gameplay
{
	public class EnemyManager : MonoBehaviour
	{
		public List<SpawnWave> waves;

		[HideInInspector] public Spawner spawner;

		bool spawnComplete = false;

		void Awake()
		{
			spawner = GetComponentInChildren<Spawner>();
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
		}

		void HandleSpawnComplete(int enemyCount)
		{
			spawnComplete = true;
		}

		void HandleEnemyDeath(Enemy enemy)
		{
			if (spawnComplete && Enemy.AliveCount == 0)
			{
				EventManager.Broadcast(new GameOverEvent(true));
			}
		}
	}
}
