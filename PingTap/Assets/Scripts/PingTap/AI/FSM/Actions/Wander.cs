using Fralle.Core.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "AI/Actions/Wander")]
	public class Wander : Action
	{
		[SerializeField] float wanderDistance = 10f;
		[SerializeField] LayerMask layerMask;

		public override void Act(IStateController ctrl)
		{
			EnemyStateController controller = (EnemyStateController)ctrl;
			if (controller.navMeshAgent.remainingDistance > 0.5f)
				return;

			Debug.Log("WANDERING");
			Vector3 newPos = RandomNavSphere(controller.transform.position);
			controller.navMeshAgent.SetDestination(newPos);
		}

		Vector3 RandomNavSphere(Vector3 origin)
		{
			Vector3 randDirection = Random.insideUnitSphere * wanderDistance + origin;
			NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, wanderDistance, layerMask);
			return navHit.position;
		}
	}
}
