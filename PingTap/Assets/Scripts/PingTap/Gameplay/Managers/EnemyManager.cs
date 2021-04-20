using Fralle.AI;
using Fralle.Core.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Gameplay
{
	public class EnemyManager : MonoBehaviour
	{
		public List<SpawnWave> Waves;

		bool spawnComplete;

		void Start()
		{
			Managers.Instance.Spawner.OnSpawnComplete += HandleSpawnComplete;
			Enemy.OnAnyEnemyDeath += HandleEnemyDeath;
		}

		public void PrepareSpawner()
		{
			var wave = Waves.PopAt(0);
			Managers.Instance.Spawner.SetSpawnDefinition(wave);
		}

		public void StartSpawner()
		{
			Managers.Instance.Spawner.StartSpawning();
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

		void OnDestroy()
		{
			Managers.Instance.Spawner.OnSpawnComplete -= HandleSpawnComplete;
			Enemy.OnAnyEnemyDeath -= HandleEnemyDeath;
		}
	}
}
