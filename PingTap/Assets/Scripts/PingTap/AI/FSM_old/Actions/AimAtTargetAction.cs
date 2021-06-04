using Fralle.Core.AI;
using Fralle.Core.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "AI/Actions/AimAtTarget")]
	public class AimAtTargetAction : Action
	{
		EnemyStateController controller;

		public override void OnEnter(IStateController ctrl)
		{
			controller = (EnemyStateController)ctrl;
		}

		public override void Tick(IStateController ctrl)
		{
			controller.AIAttack.AimAt(controller.AITargetingSystem.TargetPosition + Vector3.up);
		}

		public override void OnExit(IStateController ctrl)
		{
		}
	}
}
