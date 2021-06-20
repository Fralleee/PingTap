using UnityEngine;

public abstract class BenchmarkEvent : ScriptableObject
{
	public abstract void Run(BenchmarkController benchmarkController);
}
