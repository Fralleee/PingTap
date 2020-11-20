using Fralle.AI;
using QFSW.QC;
using UnityEngine;

namespace Fralle.Gameplay
{
	public class ConsoleCommands
	{
		[Command(aliasOverride: "spawn-enemy", description: "Spawns {x} number of enemies.")]
		public static void SpawnEnemy(int count)
		{
			Debug.Log($"Spawned {count} enemies");
			Managers.Instance.Enemy.spawner.SpawnEnemy(count);
		}

		[Command(aliasOverride: "spawn-enemy", description: "Spawns {x} number of enemies of type {y}.")]
		public static void SpawnEnemy(int count, EnemyType enemyType)
		{
			Debug.Log($"Spawned {count} {enemyType.ToString().ToLower()}s");
			Managers.Instance.Enemy.spawner.SpawnEnemyType(enemyType, count);
		}


		[Command(aliasOverride: "remove-enemies", description: "Removes {x} enemies. If no count is entered will remove ALL enemies")]
		public static void RemoveEnemies(int count = 0)
		{
			Enemy.Despawn(count);
		}

		[Command(aliasOverride: "start-match", description: "Starts match")]
		public static void StartMatch()
		{
			Managers.Instance.State.SetState(GameState.Prepare);
		}
	}
}
