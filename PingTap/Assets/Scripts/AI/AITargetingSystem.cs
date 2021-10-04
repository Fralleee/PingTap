using Fralle.Core;
using SensorToolkit;
using System.Linq;
using UnityEngine;

namespace Fralle.PingTap
{
  public class AITargetingSystem : MonoBehaviour
  {
    [Header("Configuration")]
    public float memorySpan = 4f;

    [Header("Weights")]
    public float distanceWeight = 1f;
    public float angleWeight = 1f;
    public float ageWeight = 1f;

    [Header("Debug")]
    public Color bestMemoryColor = Color.red;
    public Color memoryColor = Color.yellow;
    public bool debug;

    AISensoryMemory memory;
    AIMemory bestMemory;
    TeamController teamController;
    Sensor sensor;
    FOVCollider sensorCollider;

    public bool HasTarget => bestMemory != null && !bestMemory.DamageController.IsDead;
    public GameObject Target => bestMemory?.GameObject;
    public Vector3 TargetPosition => bestMemory?.Position ?? Vector3.zero;
    public bool TargetInSight => bestMemory?.Age < 0.5f;

    void Awake()
    {
      teamController = GetComponent<TeamController>();
      memory = new AISensoryMemory(teamController);
    }

    void Update()
    {
      memory.UpdateSenses(sensor);
      memory.ForgetMemories(memorySpan);
      EvaluateScores();
    }

    void EvaluateScores()
    {
      foreach (AIMemory m in memory.Memories.Where(x => x.Hostile))
      {
        m.Score = CalculateScore(m);
        if (bestMemory == null || m.Score > bestMemory.Score)
          bestMemory = m;
      }
    }

    static float Normalize(float value, float maxValue) => 1 - value / maxValue;

    float CalculateScore(AIMemory memory)
    {
      float distanceScore = Normalize(memory.Distance, sensorCollider.Length) * distanceWeight;
      float angleScore = Normalize(memory.Angle, sensorCollider.FOVAngle) * angleWeight;
      float ageScore = Normalize(memory.Age, memorySpan) * ageWeight;
      return distanceScore + angleScore + ageScore;
    }

    void OnValidate()
    {
      if (!sensor)
        sensor = GetComponentInChildren<TriggerSensor>();
      if (!sensorCollider)
        sensorCollider = GetComponentInChildren<FOVCollider>();
    }

    void OnDrawGizmos()
    {
      if (!debug)
        return;

      float maxScore = memory.Memories.Aggregate(float.MinValue, (current, m) => Mathf.Max(current, m.Score));

      foreach (AIMemory m in memory.Memories)
      {
        memoryColor.a = m.Score / maxScore;
        Gizmos.color = memoryColor;
        if (m == bestMemory)
          Gizmos.color = bestMemoryColor;
        Gizmos.DrawSphere(m.Position, 0.25f);
      }
    }
  }
}
