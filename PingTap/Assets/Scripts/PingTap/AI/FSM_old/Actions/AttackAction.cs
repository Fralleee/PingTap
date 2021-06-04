using CombatSystem.Combat.Damage;
using CombatSystem.Targeting;
using Fralle.AI;
using Fralle.Core;
using Fralle.Core.AI;
using UnityEngine;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "AI/Actions/Attack")]
	public class AttackAction : Action
	{
		EnemyStateController controller;

		public override void OnEnter(IStateController ctrl)
		{
			controller = (EnemyStateController)ctrl;
		}

		public override void Tick(IStateController ctrl)
		{
			controller.AIAttack.Attack();
		}

		public override void OnExit(IStateController ctrl)
		{
		}
	}
}
