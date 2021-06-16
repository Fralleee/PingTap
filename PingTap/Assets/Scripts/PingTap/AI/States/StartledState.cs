using CombatSystem.Targeting;
using Fralle.Core.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap.AI
{
	public class StartledState : IState<AIState>
	{
		public AIState identifier => AIState.Startled;

		public Vector3 position;

		AIBrain aiBrain;
		AISensor aiSensor;
		NavMeshAgent navMeshAgent;

		public StartledState(AIBrain aiBrain, AISensor aiSensor, NavMeshAgent navMeshAgent)
		{
			this.aiBrain = aiBrain;
			this.aiSensor = aiSensor;
			this.navMeshAgent = navMeshAgent;
		}

		public void OnEnter()
		{
			aiSensor.scanFrequency = aiBrain.searchScanFrequency;
			navMeshAgent.speed = aiBrain.runSpeed;

			if (NavMesh.SamplePosition(position, out NavMeshHit navMeshHit, 5f, -1))
				navMeshAgent.SetDestination(navMeshHit.position);
		}

		public void OnLogic()
		{
		}

		public void OnExit()
		{
			aiSensor.scanFrequency = aiBrain.idleScanFrequency;
			navMeshAgent.speed = aiBrain.walkSpeed;
			navMeshAgent.isStopped = true;
			navMeshAgent.ResetPath();
		}
	}
}
