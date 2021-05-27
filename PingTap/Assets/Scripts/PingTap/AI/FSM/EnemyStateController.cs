using Fralle.Core.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap
{
	public class EnemyStateController : MonoBehaviour, IStateController
	{
		[HideInInspector] public float stateTimeElapsed;

		public State currentState;
		public State remainState;

		[HideInInspector] public NavMeshAgent navMeshAgent;

		void Awake()
		{
			navMeshAgent = GetComponent<NavMeshAgent>();
		}

		void Update()
		{
			currentState.UpdateState(this);
		}

		void OnDrawGizmos()
		{
			if (currentState != null)
			{
				Gizmos.color = currentState.sceneGizmoColor;
				Gizmos.DrawWireSphere(transform.position, 10f);
			}
		}

		public void TransitionToState(State nextState)
		{
			if (nextState != remainState)
			{
				currentState = nextState;
				OnExitState();
			}
		}

		public bool CheckIfCountDownElapsed(float duration)
		{
			stateTimeElapsed += Time.deltaTime;
			return (stateTimeElapsed >= duration);
		}

		void OnExitState()
		{
			stateTimeElapsed = 0;
		}
	}
}

