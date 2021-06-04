using Fralle.Core.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "AI/Actions/TurnTowardsTarget")]
	public class TurnTowardsTargetAction : Action
	{
		EnemyStateController controller;

		public override void OnEnter(IStateController ctrl)
		{
			controller = (EnemyStateController)ctrl;
		}

		public override void Tick(IStateController ctrl)
		{
			if (controller.navMeshAgent.isStopped)
				controller.transform.LookAt(controller.AITargetingSystem.TargetPosition);
		}

		public override void OnExit(IStateController ctrl)
		{
		}
	}
}
