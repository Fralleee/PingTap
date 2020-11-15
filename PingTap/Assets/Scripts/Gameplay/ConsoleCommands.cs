using Fralle.AI;
using QFSW.QC;
using UnityEngine;

namespace Fralle.Gameplay
{
	public class ConsoleCommands : MonoBehaviour
	{
		[Command()]
		public static void SpawnEnemies(int count)
		{
			Debug.Log($"Spawned {count} enemies");
			Managers.Instance.Enemy.spawner.SpawnEnemy(count);
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
