using CombatSystem.AI;
using CombatSystem.Targeting;
using Fralle.Core.AI;
using Fralle.PingTap.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap
{
	public class BattleState : IState<AIState>
	{
		public AIState identifier => AIState.Battling;

		AIBrain aiBrain;
		AIAttack aiAttack;
		AITargetingSystem aiTargetingSystem;
		NavMeshAgent navMeshAgent;

		bool doRotate;
		float defaultStoppingDistance = 0.5f;

		public BattleState(AIBrain aiBrain, AIAttack aiAttack, AITargetingSystem aiTargetingSystem, NavMeshAgent navMeshAgent)
		{
			this.aiBrain = aiBrain;
			this.aiAttack = aiAttack;
			this.aiTargetingSystem = aiTargetingSystem;
			this.navMeshAgent = navMeshAgent;
		}

		public void OnEnter()
		{
			navMeshAgent.speed = aiBrain.walkSpeed;
			navMeshAgent.stoppingDistance = aiBrain.attackStoppingDistance;
		}

		public void OnLogic()
		{
			navMeshAgent.SetDestination(aiTargetingSystem.TargetPosition);
			aiAttack.AimAt(aiTargetingSystem.TargetPosition);
			UpdateRotation();
			aiAttack.Attack();
		}

		public void OnExit()
		{
			navMeshAgent.stoppingDistance = defaultStoppingDistance;
			navMeshAgent.isStopped = true;
			navMeshAgent.ResetPath();
		}

		void UpdateRotation()
		{
			if (navMeshAgent.velocity.magnitude > 0.1f)
				doRotate = false;
			else if (Vector3.Angle(navMeshAgent.transform.forward, aiAttack.aim.forward) > aiBrain.rotateOnAngle)
				doRotate = true;

			if (doRotate)
			{
				navMeshAgent.transform.rotation = Quaternion.Lerp(navMeshAgent.transform.rotation, aiAttack.aim.rotation, Time.deltaTime * 10f);
				if (Vector3.Angle(navMeshAgent.transform.forward, aiAttack.aim.forward) > 3f)
					return;

				navMeshAgent.transform.rotation = aiAttack.aim.rotation;
				doRotate = false;
			}
		}

	}
}
