using Fralle.AI;
using Fralle.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Benchmark/AI Combat Testing")]
public class AICombatTesting : BenchmarkEvent
{
	public int wave1EnemyCount = 10;
	public int wave2EnemyCount = 25;
	public int wave3EnemyCount = 50;
	public int wave4EnemyCount = 100;
	public float timeBetweenWaves = 30;
	public GameObject enemyPrefab;

	BenchmarkController benchmarkController;

	public override void Run(BenchmarkController benchmarkController)
	{
		this.benchmarkController = benchmarkController;
		Debug.Log("--- Running AI Combat Testing ---");
		IEnumerator[] enumerators = new IEnumerator[] { Wave1(), Wave2(), Wave3(), Wave4() };
		benchmarkController.RunEnumerators(enumerators);
	}

	IEnumerator Wave1()
	{
		Debug.Log("--- Wave 1 ---");
		SpawnEnemies(wave1EnemyCount);
		yield return new WaitForSeconds(timeBetweenWaves);
		Unit.Despawn(0);
		Debug.Log($"--- Wave completed with avg fps of {benchmarkController.CurrentFps} ---");
	}

	IEnumerator Wave2()
	{
		Debug.Log("--- Wave 2 ---");
		SpawnEnemies(wave2EnemyCount);
		yield return new WaitForSeconds(timeBetweenWaves);
		Unit.Despawn(0);
		Debug.Log($"--- Wave completed with avg fps of {benchmarkController.CurrentFps} ---");
	}

	IEnumerator Wave3()
	{
		Debug.Log("--- Wave 3 ---");
		SpawnEnemies(wave3EnemyCount);
		yield return new WaitForSeconds(timeBetweenWaves);
		Unit.Despawn(0);
		Debug.Log($"--- Wave completed with avg fps of {benchmarkController.CurrentFps} ---");
	}

	IEnumerator Wave4()
	{
		Debug.Log("--- Wave 4 ---");
		SpawnEnemies(wave4EnemyCount);
		yield return new WaitForSeconds(timeBetweenWaves);
		Unit.Despawn(0);
		Debug.Log($"--- Wave completed with avg fps of {benchmarkController.CurrentFps} ---");
		End();
	}

	void End()
	{
		Debug.Log("--- Finished running AI Combat Testing ---");
	}

	void SpawnEnemies(int count)
	{
		for (int i = 0; i < count; i++)
		{
			RandomPoint(out Vector3 position);
			var instance = Instantiate(enemyPrefab, position, Quaternion.identity);
			instance.name = (i % 2) == 0 ? "Team 1 Soldier" : "Team 2 Soldier";
			var teamController = instance.GetComponent<TeamController>();
			teamController.team = (i % 2) == 0 ? Team.Team1 : Team.Team2;
			teamController.Setup();
		}
	}

	bool RandomPoint(out Vector3 result)
	{
		for (int i = 0; i < 30; i++)
		{
			Vector3 randomPoint = Vector3.zero + Random.insideUnitSphere * 25f;
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
