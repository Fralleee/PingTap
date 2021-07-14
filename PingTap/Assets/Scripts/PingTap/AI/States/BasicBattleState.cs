using CombatSystem;
using Fralle.PingTap.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "AI/States/Battle/Basic")]
	public class BasicBattleState : BattleState
	{
		AIBrain aiBrain;
		AIAttack aiAttack;
		AITargetingSystem aiTargetingSystem;
		NavMeshAgent navMeshAgent;

		bool doRotate;
		float defaultStoppingDistance = 0.5f;

		public BasicBattleState(AIBrain aiBrain, AIAttack aiAttack, AITargetingSystem aiTargetingSystem, NavMeshAgent navMeshAgent)
		{
			this.aiBrain = aiBrain;
			this.aiAttack = aiAttack;
			this.aiTargetingSystem = aiTargetingSystem;
			this.navMeshAgent = navMeshAgent;
		}

		public override void OnEnter()
		{
			navMeshAgent.speed = aiBrain.walkSpeed;
			navMeshAgent.stoppingDistance = aiBrain.attackStoppingDistance;

			aiBrain.AlertOthers(aiTargetingSystem.TargetPosition, AIState.Chasing);
		}

		public override void OnLogic()
		{
			navMeshAgent.SetDestination(aiTargetingSystem.TargetPosition);
			aiAttack.AimAt(aiTargetingSystem.TargetPosition);
			UpdateRotation();
			aiAttack.Attack(aiTargetingSystem.TargetPosition, aiBrain.attackRange);

			if (Time.time > aiBrain.lastAlert)
				aiBrain.AlertOthers(aiTargetingSystem.TargetPosition, AIState.Chasing);
		}

		public override void OnExit()
		{
			navMeshAgent.stoppingDistance = defaultStoppingDistance;
			navMeshAgent.isStopped = true;
			navMeshAgent.ResetPath();
		}

		public override void Setup(AIBrain aiBrain)
		{
			this.aiBrain = aiBrain;
			aiAttack = aiBrain.GetComponent<AIAttack>();
			aiTargetingSystem = aiBrain.GetComponent<AITargetingSystem>();
			navMeshAgent = aiBrain.GetComponent<NavMeshAgent>();
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
