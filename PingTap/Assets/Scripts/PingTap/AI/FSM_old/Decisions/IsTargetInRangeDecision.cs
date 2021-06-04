using Fralle.Core.AI;
using UnityEngine;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "AI/Decisions/IsTargetInRange")]
	public class IsTargetInRangeDecision : Decision
	{
		public override bool Decide(IStateController ctrl)
		{
			EnemyStateController controller = (EnemyStateController)ctrl;

			return controller.AITargetingSystem.TargetDistance <= controller.AITargetingSystem.AttackRange;
		}
	}
}
