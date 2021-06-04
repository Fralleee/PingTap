using Fralle.Core.AI;
using UnityEngine;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "AI/Decisions/IsTargetAvailable")]
	public class IsTargetAvailableDecision : Decision
	{
		public override bool Decide(IStateController ctrl)
		{
			EnemyStateController controller = (EnemyStateController)ctrl;
			
			return controller.AITargetingSystem.HasTarget;
		}
	}
}
