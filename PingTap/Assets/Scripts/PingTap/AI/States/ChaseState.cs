
using CombatSystem.Targeting;
using Fralle.Core.HFSM;
using Fralle.PingTap.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap
{
	public class ChaseState : IState<AIState>
	{
		public AIState identifier => AIState.Chasing;

		public Vector3 position;

		AITargetingSystem aiTargetingSystem;
		NavMeshAgent navMeshAgent;

		float speed = 6f;
		float defaultSpeed;

		public ChaseState(AITargetingSystem aiTargetingSystem, NavMeshAgent navMeshAgent)
		{
			this.aiTargetingSystem = aiTargetingSystem;
			this.navMeshAgent = navMeshAgent;
		}

		public void OnEnter()
		{
			navMeshAgent.speed = speed;
		}

		public void OnLogic()
		{
			navMeshAgent.SetDestination(aiTargetingSystem.TargetPosition);
		}

		public void OnExit()
		{
			navMeshAgent.speed = defaultSpeed;
			navMeshAgent.isStopped = true;
			navMeshAgent.ResetPath();
		}
	}
}
