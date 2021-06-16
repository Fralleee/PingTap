using CombatSystem.Targeting;
using Fralle.Core.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap.AI
{
	public class WanderState : IState<AIState>
	{
		public AIState identifier => _identifier;
		AIState _identifier;

		AIBrain aiBrain;
		AISensor aiSensor;
		NavMeshAgent navMeshAgent;

		bool searchState;
		float wanderDistance = 10f;


		public WanderState(AIBrain aiBrain, AISensor aiSensor, NavMeshAgent navMeshAgent, bool searching = false)
		{
			this.aiBrain = aiBrain;
			this.aiSensor = aiSensor;
			this.navMeshAgent = navMeshAgent;
			_identifier = AIState.Wandering;
			searchState = searching;

			if (searchState)
				_identifier = AIState.Searching;
		}

		public void OnEnter()
		{
			navMeshAgent.speed = searchState ? aiBrain.runSpeed : aiBrain.walkSpeed;
			aiSensor.scanFrequency = searchState ? aiBrain.searchScanFrequency : aiBrain.idleScanFrequency;
		}

		public void OnLogic()
		{
			if (navMeshAgent.remainingDistance > 0.5f)
				return;

			Vector3 newPos = RandomNavSphere(navMeshAgent.transform.position);
			navMeshAgent.SetDestination(newPos);
		}

		public void OnExit()
		{
			aiSensor.scanFrequency = aiBrain.idleScanFrequency;
			navMeshAgent.speed = aiBrain.walkSpeed;
			navMeshAgent.isStopped = true;
			navMeshAgent.ResetPath();
		}

		Vector3 RandomNavSphere(Vector3 origin)
		{
			Vector3 randDirection = Random.insideUnitSphere * wanderDistance + origin;
			NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, wanderDistance, -1);
			return navHit.position;
		}
	}
}
