using CombatSystem.Targeting;
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
		}

		public override void OnLogic()
		{
			navMeshAgent.SetDestination(aiTargetingSystem.TargetPosition);
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
