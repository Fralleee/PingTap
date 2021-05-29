using Fralle.Core.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "AI/Actions/Chase")]
	public class Chase : Action
	{
		EnemyStateController controller;

		public override void OnEnter(IStateController ctrl)
		{
			controller = (EnemyStateController)ctrl;
		}

		public override void Tick(IStateController ctrl)
		{
			controller.navMeshAgent.SetDestination(controller.target.transform.position);
		}

		public override void OnExit(IStateController ctrl)
		{
			controller.navMeshAgent.isStopped = true;
			controller.navMeshAgent.ResetPath();
		}
	}
}
