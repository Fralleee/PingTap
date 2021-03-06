using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap.AI
{
	[CreateAssetMenu(menuName = "AI/States/Search/Basic")]
	public class BasicSearchState : SearchState
	{
		AIBrain aiBrain;
		NavMeshAgent navMeshAgent;

		float searchDistance = 10f;

		public override void OnEnter()
		{
			navMeshAgent.speed = aiBrain.runSpeed;
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
			navMeshAgent.speed = aiBrain.walkSpeed;
			navMeshAgent.isStopped = true;
			navMeshAgent.ResetPath();
		}

		public override void Setup(AIBrain aiBrain)
		{
			this.aiBrain = aiBrain;
			navMeshAgent = aiBrain.GetComponent<NavMeshAgent>();
		}

		Vector3 RandomNavSphere(Vector3 origin)
		{
			Vector3 randDirection = Random.insideUnitSphere * searchDistance + origin;
			NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, searchDistance, -1);
			return navHit.position;
		}
	}
}
