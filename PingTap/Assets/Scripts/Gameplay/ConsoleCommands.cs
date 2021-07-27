namespace Fralle.Gameplay
{
	public class ConsoleCommands
	{
		//[Command(aliasOverride: "spawn-enemy", description: "Spawns {x} number of enemies.")]
		//public static void SpawnEnemy(int count)
		//{
		//	Debug.Log($"Spawned {count} enemies");
		//	Managers.Instance.Spawner.SpawnEnemy(count);
		//}

		//[Command(aliasOverride: "spawn-enemy", description: "Spawns {x} number of enemies of type {y}.")]
		//public static void SpawnEnemy(int count, EnemyType enemyType)
		//{
		//	Debug.Log($"Spawned {count} {enemyType.ToString().ToLower()}s");
		//	Managers.Instance.Spawner.SpawnEnemyType(enemyType, count);
		//}


		//[Command(aliasOverride: "remove-enemies", description: "Removes {x} enemies. If no count is entered will remove ALL enemies")]
		//public static void RemoveEnemies(int count = 0)
		//{
		//	Enemy.Despawn(count);
		//}

		//[Command(aliasOverride: "start-match", description: "Starts match")]
		//public static void StartMatch()
		//{
		//	Managers.Instance.State.SetState(MatchState.Prepare);
		//}


		//[Command(aliasOverride: "limit-fps", description: "Sets the frame rate limit")]
		//public static void LimitFPS(int limit)
		//{
		//	Debug.Log($"Frame rate limit set to {limit}");
		//	Application.targetFrameRate = limit;
		//}

		//[Command(aliasOverride: "upgrade_stats", description: "Upgrades a stat on the player")]
		//public static void UpgradeStat(string name, int count)
		//{
		//	Debug.LogWarning("Not implemented yet");
		//	//var stats = Object.FindObjectOfType<Stats>();
		//	//for (int i = 0; i < count; i++)
		//	//{
		//	//	stats.UpgradeStatistic(name);
		//	//}
		//	//Debug.Log($"Upgrading {name} to {stats.GetStatisticLevel(name)}");
		//}
	}
}
