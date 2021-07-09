using CombatSystem.Targeting;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap.AI
{
	[CreateAssetMenu(menuName = "AI/States/Startled/Basic")]
	public class BasicStartledState : StartledState
	{
		AIBrain aiBrain;
		AISensor aiSensor;
		NavMeshAgent navMeshAgent;

		public BasicStartledState(AIBrain aiBrain, AISensor aiSensor, NavMeshAgent navMeshAgent)
		{
			this.aiBrain = aiBrain;
			this.aiSensor = aiSensor;
			this.navMeshAgent = navMeshAgent;
		}

		public override void OnEnter()
		{
			aiSensor.scanFrequency = aiBrain.searchScanFrequency;
			navMeshAgent.speed = aiBrain.runSpeed;

			if (NavMesh.SamplePosition(origin, out NavMeshHit navMeshHit, 5f, -1))
				navMeshAgent.SetDestination(navMeshHit.position);

			aiBrain.AlertOthers(origin, AIState.Startled);
		}

		public override void OnLogic()
		{
		}

		public override void OnExit()
		{
			aiSensor.scanFrequency = aiBrain.idleScanFrequency;
			navMeshAgent.speed = aiBrain.walkSpeed;
			navMeshAgent.isStopped = true;
			navMeshAgent.ResetPath();
		}

		public override void Setup(AIBrain aiBrain)
		{
			this.aiBrain = aiBrain;
			aiSensor = aiBrain.GetComponent<AISensor>();
			navMeshAgent = aiBrain.GetComponent<NavMeshAgent>();
		}
	}
}
