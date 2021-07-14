using CombatSystem;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap.AI
{
	[CreateAssetMenu(menuName = "AI/States/Chase/Basic")]
	public class BasicChaseState : ChaseState
	{
		AIBrain aiBrain;
		AITargetingSystem aiTargetingSystem;
		NavMeshAgent navMeshAgent;

		public BasicChaseState(AIBrain aiBrain, AITargetingSystem aiTargetingSystem, NavMeshAgent navMeshAgent)
		{
			this.aiBrain = aiBrain;
			this.aiTargetingSystem = aiTargetingSystem;
			this.navMeshAgent = navMeshAgent;
		}

		public override void OnEnter()
		{
			navMeshAgent.speed = aiBrain.runSpeed;

			aiBrain.AlertOthers(aiTargetingSystem.TargetPosition, AIState.Chasing);
		}

		public override void OnLogic()
		{
			navMeshAgent.SetDestination(aiTargetingSystem.TargetPosition);

			if (Time.time > aiBrain.lastAlert)
				aiBrain.AlertOthers(aiTargetingSystem.TargetPosition, AIState.Chasing);
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
			aiTargetingSystem = aiBrain.GetComponent<AITargetingSystem>();
			navMeshAgent = aiBrain.GetComponent<NavMeshAgent>();
		}
	}
}
