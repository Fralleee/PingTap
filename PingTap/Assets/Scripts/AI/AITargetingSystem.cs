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

		public GameObject lockedOnTarget { get; private set; }
		public float AttackRange => 10f;
		public bool HasTarget => bestMemory != null && !bestMemory.damageController.IsDead;
		public GameObject Target => bestMemory?.gameObject;
		public Vector3 TargetPosition => bestMemory != null ? bestMemory.position : Vector3.zero;
		public bool TargetInSight => bestMemory?.Age < 0.5f;
		public float TargetDistance => bestMemory.distance;

		public void LockOn()
		{
			lockedOnTarget = Target;
		}

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
			foreach (var memory in memory.memories.Where(x => x.hostile))
			{
				memory.score = CalculateScore(memory);
				if (bestMemory == null || memory.score > bestMemory.score)
					bestMemory = memory;
			}
		}

		float Normalize(float value, float maxValue) => 1 - value / maxValue;

		float CalculateScore(AIMemory memory)
		{
			float distanceScore = Normalize(memory.distance, sensorCollider.Length) * distanceWeight;
			float angleScore = Normalize(memory.angle, sensorCollider.FOVAngle) * angleWeight;
			float ageScore = Normalize(memory.Age, memorySpan) * ageWeight;
			return distanceScore + angleScore + ageScore;
		}

		void OnValidate()
		{
			if (sensor == null)
				sensor = GetComponentInChildren<TriggerSensor>();
			if (sensorCollider == null)
				sensorCollider = GetComponentInChildren<FOVCollider>();
		}

		void OnDrawGizmos()
		{
			if (debug)
			{
				float maxScore = float.MinValue;
				foreach (var memory in memory.memories)
					maxScore = Mathf.Max(maxScore, memory.score);

				foreach (var memory in memory.memories)
				{
					memoryColor.a = memory.score / maxScore;
					Gizmos.color = memoryColor;
					if (memory == bestMemory)
						Gizmos.color = bestMemoryColor;
					Gizmos.DrawSphere(memory.position, 0.25f);
				}
			}
		}
	}
}
