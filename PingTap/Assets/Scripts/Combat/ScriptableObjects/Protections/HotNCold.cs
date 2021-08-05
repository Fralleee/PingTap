using System.Linq;
using UnityEngine;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "PlayerAttack/Protection/Hot and cold")]
	public class HotNCold : Protection
	{
		public override ProtectionResult RunProtection(DamageData damageData, DamageController damageController, DamageEffectHandler effectHandler)
		{
			var fireToWater = damageData.Element == Element.Fire && effectHandler.DamageEffects.Any(x => x.Element == Element.Water);
			if (fireToWater)
				return new ProtectionResult() { EffectProtection = EffectProtection.Ignore, DamageData = damageData };

			var waterToFire = damageData.Element == Element.Water && effectHandler.DamageEffects.Any(x => x.Element == Element.Fire);
			if (waterToFire)
				return new ProtectionResult() { EffectProtection = EffectProtection.Ignore, DamageData = damageData };

			damageData.DamageAmount = 0;
			return new ProtectionResult() { EffectProtection = EffectProtection, DamageData = damageData };
		}
	}
}
