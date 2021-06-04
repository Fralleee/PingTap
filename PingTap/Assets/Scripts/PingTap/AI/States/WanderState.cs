using CombatSystem.Combat.Damage;
using CombatSystem.Targeting;
using Fralle.Core.HFSM;
using Fralle.PingTap.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap
{
	public class WanderState : IState<AIState>
	{
		public AIState identifier => _identifier;
		AIState _identifier;

		int scanFrequency = 1;
		float wanderDistance = 5f;
		float wanderSpeed = 2f;

		int defaultScanFrequency;
		float defaultSpeed;

		AISensor aiSensor;
		NavMeshAgent navMeshAgent;


		public WanderState(AISensor aiSensor, NavMeshAgent navMeshAgent, bool searching = false)
		{
			this.aiSensor = aiSensor;
			this.navMeshAgent = navMeshAgent;
			_identifier = AIState.Wandering;

			if (searching)
			{
				_identifier = AIState.Searching;
				scanFrequency = 3;
				wanderSpeed = 5f;
			}
		}

		public void OnEnter()
		{
			navMeshAgent.speed = wanderSpeed;
			aiSensor.scanFrequency = scanFrequency;
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
			aiSensor.scanFrequency = defaultScanFrequency;
			navMeshAgent.speed = defaultSpeed;
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
