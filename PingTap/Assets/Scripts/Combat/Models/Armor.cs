using System;
using System.Linq;
using UnityEngine;

namespace Fralle.PingTap
{
	[Serializable]
	public class Armor
	{
		public float DamageMultiplier => 1 - 0.06f * ArmorValue / (1 + 0.06f * ArmorValue);
		public int ArmorValue;
		public Protection Protection;

		[SerializeField] ArmorElementModifier Fire = new ArmorElementModifier(Element.Fire);
		[SerializeField] ArmorElementModifier Water = new ArmorElementModifier(Element.Water);
		[SerializeField] ArmorElementModifier Earth = new ArmorElementModifier(Element.Earth);
		[SerializeField] ArmorElementModifier Lightning = new ArmorElementModifier(Element.Lightning);

		public DamageData Protect(DamageData damageData, DamageController damageController)
		{
			ProtectionResult result = RunProtection(damageData, damageController);
			result.DamageData.DamageAmount = CalculateDamage(result.DamageData);
			if (result.EffectProtection == EffectProtection.Block)
				result.DamageData.Effects = new DamageEffect[0];
			else
				damageData.Effects = damageData.Effects.Select(CalculateEffect).ToArray();
			return result.DamageData;
		}

		float CalculateDamage(DamageData damageData)
		{
			ArmorElementModifier armorElementModifier = GetModifier(damageData.Element);
			float modifier = armorElementModifier?.Modifier ?? 1;
			float damageAmount = damageData.DamageAmount * modifier * DamageMultiplier;
			return damageAmount;
		}

		DamageEffect CalculateEffect(DamageEffect effect)
		{
			ArmorElementModifier armorElementModifier = GetModifier(effect.Element);
			return armorElementModifier != null ? effect.Recalculate(armorElementModifier.Modifier) : effect;
		}

		ProtectionResult RunProtection(DamageData damageData, DamageController damageController)
		{
			return !Protection ? new ProtectionResult() { EffectProtection = EffectProtection.Ignore, DamageData = damageData } : Protection.RunProtection(damageData, damageController, damageController.effectHandler);
		}

		ArmorElementModifier GetModifier(Element element)
		{
			switch (element)
			{
				case Element.Fire:
					return Fire;
				case Element.Water:
					return Water;
				case Element.Earth:
					return Earth;
				case Element.Lightning:
					return Lightning;
				default:
					return null;
			}
		}
	}
}
