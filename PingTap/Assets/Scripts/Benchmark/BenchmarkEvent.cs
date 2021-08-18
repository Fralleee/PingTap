using UnityEngine;

namespace Fralle.PingTap.Benchmark
{
  public abstract class BenchmarkEvent : ScriptableObject
  {
    public abstract void Run(BenchmarkController benchmarkController);
  }
}
