using UnityEngine;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "PlayerAttack/Effect/DamageData over time")]
	public class DotEffect : DamageEffect
	{
		[Header("Dot specific")] public DamageType DamageApplication = DamageType.Flat;
		public float DamageModifier = 1f;
		[HideInInspector] public float LastDamageTimer;

		public override DamageEffect Append(DamageEffect oldEffect = null)
		{
			base.Append(oldEffect);
			if (oldEffect != null)
				LastDamageTimer = ((DotEffect)oldEffect).LastDamageTimer;
			return this;
		}

		public override void Tick(DamageController damageController)
		{
			base.Tick(damageController);
			LastDamageTimer += UnityEngine.Time.deltaTime;

			if (!(LastDamageTimer > 1))
				return;

			LastDamageTimer -= 1f;
			var damage = BaseDamageModifier * DamageConverter.AsDamageModifier(DamageModifier / Time, DamageApplication, damageController, WeaponDamage);
			damageController.ReceiveAttack(new DamageData()
			{
				Attacker = Attacker,
				Element = Element,
				HitAngle = -1,
				Position = damageController.transform.position,
				DamageFromHit = false,
				DamageAmount = damage * Stacks
			});
		}

		public override DamageEffect Recalculate(float modifier)
		{
			DamageModifier *= modifier;
			return this;
		}
	}
}
