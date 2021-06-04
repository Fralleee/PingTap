using CombatSystem.Combat.Damage;
using CombatSystem.Targeting;
using Fralle.Core.HFSM;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap.AI
{
	public class StartledState : IState<AIState>
	{
		public AIState identifier => AIState.Startled;

		public Vector3 position;

		int scanFrequency = 3;
		float speed = 6f;

		int defaultScanFrequency;
		float defaultSpeed;

		AISensor aiSensor;
		NavMeshAgent navMeshAgent;

		public StartledState(AISensor aiSensor, NavMeshAgent navMeshAgent)
		{
			this.aiSensor = aiSensor;
			this.navMeshAgent = navMeshAgent;
		}

		public void OnEnter()
		{
			aiSensor.scanFrequency = scanFrequency;
			navMeshAgent.speed = speed;

			if (NavMesh.SamplePosition(position, out NavMeshHit navMeshHit, 5f, -1))
				navMeshAgent.SetDestination(navMeshHit.position);
		}

		public void OnLogic()
		{
		}

		public void OnExit()
		{
			aiSensor.scanFrequency = defaultScanFrequency;
			navMeshAgent.speed = defaultSpeed;
			navMeshAgent.isStopped = true;
			navMeshAgent.ResetPath();
		}
	}
}
