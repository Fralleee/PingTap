using Fralle.Core.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "AI/Actions/Follow")]
	public class FollowAction : Action
	{
		[SerializeField] float speed = 4f;
		[SerializeField] float minDistance = 5f;

		EnemyStateController controller;
		float defaultSpeed = 0f;

		public override void OnEnter(IStateController ctrl)
		{
			controller = (EnemyStateController)ctrl;
			defaultSpeed = controller.navMeshAgent.speed;
			controller.navMeshAgent.speed = speed;
		}

		public override void Tick(IStateController ctrl)
		{
			if (minDistance == 0f)
			{
				controller.navMeshAgent.SetDestination(controller.AITargetingSystem.TargetPosition);
				return;
			}

			var distance = Vector3.Distance(controller.transform.position, controller.AITargetingSystem.TargetPosition);
			if (distance > minDistance)
				controller.navMeshAgent.SetDestination(controller.AITargetingSystem.TargetPosition);
			else
			{
				controller.navMeshAgent.isStopped = true;
				controller.navMeshAgent.ResetPath();
			}
		}

		public override void OnExit(IStateController ctrl)
		{
			controller.navMeshAgent.speed = defaultSpeed;
			controller.navMeshAgent.isStopped = true;
			controller.navMeshAgent.ResetPath();
		}
	}
}
