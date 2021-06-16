using CombatSystem.Targeting;
using Fralle.Core.AI;
using UnityEngine.AI;

namespace Fralle.PingTap.AI
{
	public class ChaseState : IState<AIState>
	{
		public AIState identifier => AIState.Chasing;

		AIBrain aiBrain;
		AITargetingSystem aiTargetingSystem;
		NavMeshAgent navMeshAgent;

		public ChaseState(AIBrain aiBrain, AITargetingSystem aiTargetingSystem, NavMeshAgent navMeshAgent)
		{
			this.aiBrain = aiBrain;
			this.aiTargetingSystem = aiTargetingSystem;
			this.navMeshAgent = navMeshAgent;
		}

		public void OnEnter()
		{
			navMeshAgent.speed = aiBrain.runSpeed;
		}

		public void OnLogic()
		{
			navMeshAgent.SetDestination(aiTargetingSystem.TargetPosition);
		}

		public void OnExit()
		{
			navMeshAgent.speed = aiBrain.walkSpeed;
			navMeshAgent.isStopped = true;
			navMeshAgent.ResetPath();
		}
	}
}
