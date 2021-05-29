using Fralle.Core.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "AI/Decisions/IsTargetAvailable")]
	public class IsTargetAvailable : Decision
	{
		public override bool Decide(IStateController ctrl)
		{
			EnemyStateController controller = (EnemyStateController)ctrl;
			return controller.target != null && !controller.target.IsDead;
		}
	}
}
