using CombatSystem.Targeting;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap.AI
{
	[CreateAssetMenu(menuName = "AI/States/Wander/Basic")]
	public class BasicWanderState : WanderState
	{
		AIBrain aiBrain;
		AISensor aiSensor;
		NavMeshAgent navMeshAgent;

		float wanderDistance = 10f;

		public override void OnEnter()
		{
			navMeshAgent.speed = aiBrain.walkSpeed;
			aiSensor.scanFrequency = aiBrain.idleScanFrequency;
		}

		public override void OnLogic()
		{
			if (navMeshAgent.remainingDistance > 0.5f)
				return;

			Vector3 newPos = RandomNavSphere(navMeshAgent.transform.position);
			navMeshAgent.SetDestination(newPos);
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

		Vector3 RandomNavSphere(Vector3 origin)
		{
			Vector3 randDirection = Random.insideUnitSphere * wanderDistance + origin;
			NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, wanderDistance, -1);
			return navHit.position;
		}
	}
}