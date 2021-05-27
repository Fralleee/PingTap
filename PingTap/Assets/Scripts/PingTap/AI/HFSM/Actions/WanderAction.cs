using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap
{
	public class WanderAction : StateMachineBehaviour
	{
		NavMeshAgent navMeshAgent;

		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Debug.Log("Wander::Enter");
			navMeshAgent = animator.GetComponent<NavMeshAgent>();
			SetNextPosition(navMeshAgent, animator.transform.position);
		}

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Debug.Log($"Wander::Update HasTarget:{animator.GetBool("HasTarget")}");
			if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
			{
				SetNextPosition(navMeshAgent, animator.transform.position);
			}
		}

		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Debug.Log("Wander::Exit");
			navMeshAgent.isStopped = true;
			navMeshAgent.ResetPath();
		}

		void SetNextPosition(NavMeshAgent navMeshAgent, Vector3 origin)
		{
			navMeshAgent.SetDestination(CalculateRandomNavmeshPosition(origin));
		}

		Vector3 CalculateRandomNavmeshPosition(Vector3 origin)
		{
			float distance = Random.Range(1f, 10f);
			Vector3 randomDirection = Random.insideUnitSphere * distance;
			randomDirection += origin;
			NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, distance, -1);

			return navHit.position;
		}
	}
}
