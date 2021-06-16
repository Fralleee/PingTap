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
		public float walkSpeed = 2f;
		public float runSpeed = 6f;
		public int idleScanFrequency = 1;
		public int searchScanFrequency = 4;

		[Header("Debug")]
		[SerializeField] bool debugTransitions;

		[Header("Gizmo meshes")]
		[SerializeField] Mesh wanderingMesh;
		[SerializeField] Mesh searchingMesh;
		[SerializeField] Mesh startledMesh;
		[SerializeField] Mesh chasingMesh;
		[SerializeField] Mesh battlingMesh;

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
			switch (currentState)
			{
				case AIState.Wandering:
					Gizmos.DrawMesh(wanderingMesh, transform.position + Vector3.up * 2.5f, transform.rotation);
					break;
				case AIState.Searching:
					Gizmos.DrawMesh(searchingMesh, transform.position + Vector3.up * 2.5f, transform.rotation);
					break;
				case AIState.Startled:
					Gizmos.DrawMesh(startledMesh, transform.position + Vector3.up * 2.5f, transform.rotation);
					break;
				case AIState.Chasing:
					Gizmos.DrawMesh(chasingMesh, transform.position + Vector3.up * 2.5f, transform.rotation);
					break;
				case AIState.Battling:
					Gizmos.DrawMesh(battlingMesh, transform.position + Vector3.up * 2.5f, transform.rotation);
					break;
			}
		}
	}
}
