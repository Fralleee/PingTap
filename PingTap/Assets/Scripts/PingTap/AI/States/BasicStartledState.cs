using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap.AI
{
	[CreateAssetMenu(menuName = "AI/States/Startled/Basic")]
	public class BasicStartledState : StartledState
	{
		AIBrain aiBrain;
		NavMeshAgent navMeshAgent;

		public BasicStartledState(AIBrain aiBrain, NavMeshAgent navMeshAgent)
		{
			this.aiBrain = aiBrain;
			this.navMeshAgent = navMeshAgent;
		}

		public override void OnEnter()
		{
			navMeshAgent.speed = aiBrain.runSpeed;
		}

		public override void OnLogic()
		{
		}

		public override void OnExit()
		{
			navMeshAgent.speed = aiBrain.walkSpeed;
			navMeshAgent.isStopped = true;
			navMeshAgent.ResetPath();
		}

		public override void Setup(AIBrain aiBrain)
		{
			this.aiBrain = aiBrain;
			navMeshAgent = aiBrain.GetComponent<NavMeshAgent>();
		}

		public override void NewOrigin(Vector3 origin)
		{
			if (NavMesh.SamplePosition(origin, out NavMeshHit navMeshHit, 5f, -1))
				navMeshAgent.SetDestination(navMeshHit.position);

			aiBrain.AlertOthers(origin, AIState.Startled);
		}
	}
}
