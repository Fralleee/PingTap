using Fralle.Core.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "AI/Actions/Wander")]
	public class WanderAction : Action
	{
		[SerializeField] float wanderDistance = 5f;
		[SerializeField] float wanderSpeed = 2f;
		[SerializeField] LayerMask layerMask;

		EnemyStateController controller;
		float defaultSpeed = 0f;

		public override void OnEnter(IStateController ctrl)
		{
			controller = (EnemyStateController)ctrl;
			defaultSpeed = controller.navMeshAgent.speed;
			controller.navMeshAgent.speed = wanderSpeed;
		}

		public override void Tick(IStateController ctrl)
		{
			if (controller.navMeshAgent.remainingDistance > 0.5f)
				return;

			Vector3 newPos = RandomNavSphere(controller.transform.position);
			controller.navMeshAgent.SetDestination(newPos);
		}

		public override void OnExit(IStateController ctrl)
		{
			controller.navMeshAgent.speed = defaultSpeed;
			controller.navMeshAgent.isStopped = true;
			controller.navMeshAgent.ResetPath();
		}

		Vector3 RandomNavSphere(Vector3 origin)
		{
			Vector3 randDirection = Random.insideUnitSphere * wanderDistance + origin;
			NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, wanderDistance, layerMask);
			return navHit.position;
		}
	}
}
