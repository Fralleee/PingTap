using Fralle.Core.AI;
using Fralle.Core.Attributes;
using UnityEngine;

namespace Fralle.PingTap.AI
{
	public class AIBrain : MonoBehaviour
	{
		[Readonly] public AIState currentState;

		[Header("Configuration")]
		[SerializeField] AIPersonality personality;

		[Header("Range - Distance")]
		public float attackRange = 10f;
		public float attackStoppingDistance = 5f;

		[Header("Movement")]
		public float walkSpeed = 2f;
		public float runSpeed = 6f;
		public float rotateOnAngle = 35;

		[Header("Scanning")]
		public int idleScanFrequency = 1;
		public int searchScanFrequency = 4;

		[Header("Debug")]
		[SerializeField] bool debugTransitions;

		StateMachine<AIState> stateMachine;

		void Awake()
		{
			personality = personality.CreateInstance();
		}

		void Start()
		{
			stateMachine = new StateMachine<AIState>();
			stateMachine.OnTransition += OnTransition;
			personality.Load(this, stateMachine);
		}

		void Update()
		{
			stateMachine.OnLogic();
		}

		void OnTransition(IState<AIState> newState)
		{
			if (debugTransitions)
				Debug.Log($"Transitioned from {currentState} to {newState.identifier}");

			currentState = newState.identifier;
		}

		void OnDrawGizmos()
		{
			Gizmos.color = currentState.Color();
			Gizmos.DrawSphere(transform.position + Vector3.up * 2.5f, 0.25f);
		}
	}
}
