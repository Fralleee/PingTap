using Fralle.Core;
using Fralle.PingTap.AI;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap.Benchmark
{
	[CreateAssetMenu(menuName = "Benchmark/AI Difficulty Testing")]
	public class AIDifficultyTesting : BenchmarkEvent
	{
		public int waveEnemyCount = 20;
		public GameObject enemyPrefab;

		public override void Run(BenchmarkController benchmarkController)
		{
			Debug.Log("--- Running AI Difficulty Testing ---");
			IEnumerator[] enumerators = new IEnumerator[] { Wave1(), Wave2(), Wave3(), Wave4() };
			benchmarkController.RunEnumerators(enumerators);
		}

		IEnumerator Wave1()
		{
			Debug.Log("--- Wave 1 Easy/Easy ---");
			for (int i = 0; i < waveEnemyCount; i++)
			{
				yield return new WaitForEndOfFrame();
				SpawnEnemy(i, AIDifficulty.Easy, AIDifficulty.Easy);
			}
			yield return new WaitForSeconds(25);
			// Despawns enemies
			Debug.Log($"--- Wave completed ---");
		}

		IEnumerator Wave2()
		{
			Debug.Log("--- Wave 2 Easy/Normal ---");
			for (int i = 0; i < waveEnemyCount; i++)
			{
				yield return new WaitForEndOfFrame();
				SpawnEnemy(i, AIDifficulty.Easy, AIDifficulty.Normal);
			}
			yield return new WaitForSeconds(20);
			// Despawns enemies
			Debug.Log($"--- Wave completed ---");
		}

		IEnumerator Wave3()
		{
			Debug.Log("--- Wave 3 Easy/Hard ---");
			for (int i = 0; i < waveEnemyCount; i++)
			{
				yield return new WaitForEndOfFrame();
				SpawnEnemy(i, AIDifficulty.Easy, AIDifficulty.Hard);
			}
			yield return new WaitForSeconds(15);
			// Despawns enemies
			Debug.Log($"--- Wave completed ---");
		}

		IEnumerator Wave4()
		{
			Debug.Log("--- Wave 4 Easy/Impossible ---");
			for (int i = 0; i < waveEnemyCount; i++)
			{
				yield return new WaitForEndOfFrame();
				SpawnEnemy(i, AIDifficulty.Easy, AIDifficulty.Impossible);
			}
			yield return new WaitForSeconds(10);
			// Despawns enemies
			Debug.Log($"--- Wave completed ---");
			End();
		}

		void End()
		{
			Debug.Log("--- Finished running AI Combat Testing ---");
		}

		void SpawnEnemy(int i, AIDifficulty difficulty1, AIDifficulty difficulty2)
		{
			RandomPoint(out Vector3 position);
			var instance = Instantiate(enemyPrefab, position, Quaternion.identity);
			instance.name = i % 2 == 0 ? "Team 1 Soldier" : "Team 2 Soldier";

			var aiBrain = instance.GetComponent<AIBrain>();
			aiBrain.difficulty = i % 2 == 0 ? difficulty1 : difficulty2;

			var teamController = instance.GetComponent<TeamController>();
			teamController.team = i % 2 == 0 ? Team.Team1 : Team.Team2;
			teamController.Setup();
		}

		bool RandomPoint(out Vector3 result)
		{
			for (int i = 0; i < 30; i++)
			{
				Vector3 randomPoint = Vector3.zero + Random.insideUnitSphere * 15f;
				if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
				{
					result = hit.position;
					return true;
				}
			}
			result = Vector3.zero;
			return false;
		}
	}
}
