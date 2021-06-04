using Fralle.Core.AI;
using UnityEngine;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "AI/Decisions/IsTargetInSight")]
	public class IsTargetInSightDecision : Decision
	{
		public override bool Decide(IStateController ctrl)
		{
			EnemyStateController controller = (EnemyStateController)ctrl;

			return controller.AITargetingSystem.TargetInSight;
		}
	}
}
