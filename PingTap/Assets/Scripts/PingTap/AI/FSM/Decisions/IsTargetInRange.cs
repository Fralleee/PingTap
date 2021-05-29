using Fralle.Core.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "AI/Decisions/IsTargetInRange")]
	public class IsTargetInRange : Decision
	{
		public override bool Decide(IStateController ctrl)
		{
			EnemyStateController controller = (EnemyStateController)ctrl;
			return Vector3.Distance(controller.target.transform.position, controller.transform.position) <= controller.AttackRange;
		}
	}
}
