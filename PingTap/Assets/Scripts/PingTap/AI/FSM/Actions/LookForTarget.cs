using CombatSystem.Combat.Damage;
using CombatSystem.Targeting;
using Fralle.AI;
using Fralle.Core;
using Fralle.Core.AI;
using UnityEngine;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "AI/Actions/Look for target")]
	public class LookForTarget : Action
	{
		[SerializeField] float lookDelay = 2f;

		[BitMask(typeof(TargetRequirement))]
		public TargetRequirement targetRequirement;

		EnemyStateController controller;

		float lookTimer;

		public override void OnEnter(IStateController ctrl)
		{
			controller = (EnemyStateController)ctrl;

			FindTarget();
			lookTimer = lookDelay;
		}

		public override void Tick(IStateController controller)
		{
			lookTimer -= Time.deltaTime;
			if (lookTimer > 0)
				return;

			FindTarget();
			lookTimer = lookDelay;
		}

		public override void OnExit(IStateController controller)
		{
		}

		void FindTarget()
		{
			DamageController[] targets = TargetingHelpers.FindTargetsInRange(controller.DamageController.LayerMap.Hostiles, 10f, controller.transform.position);
			if (targets.Length > 0)
				controller.target = targets[0];
		}
	}
}
