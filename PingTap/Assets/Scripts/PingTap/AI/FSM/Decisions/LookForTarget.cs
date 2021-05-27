using CombatSystem.Combat.Damage;
using CombatSystem.Targeting;
using Fralle.Core;
using Fralle.Core.AI;
using UnityEngine;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "AI/Decisions/Look")]
	public class LookForTarget : Decision
	{
		[SerializeField] float lookDelay = 2f;
		[SerializeField] LayerMask layerMask;

		[BitMask(typeof(TargetRequirement))]
		public TargetRequirement targetRequirement;

		float lookTimer;

		public override bool Decide(IStateController ctrl)
		{
			lookTimer -= Time.deltaTime;
			if (lookTimer > 0)
				return false;

			return FindTarget(ctrl);
		}

		bool FindTarget(IStateController ctrl)
		{
			lookTimer = lookDelay;

			EnemyStateController controller = (EnemyStateController)ctrl;
			DamageController[] targets = TargetingHelpers.FindTargetsInRange(layerMask, 10f, controller.transform.position);

			return targets.Length > 0;
		}
	}
}
