using CombatSystem.Combat.Damage;
using CombatSystem.Effect;
using CombatSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CombatSystem.Defense
{
  [Serializable]
  public class Armor
  {
    public float DamageMultiplier => 1 - 0.06f * ArmorValue / (1 + 0.06f * ArmorValue);

    public int ArmorValue;
    public List<ArmorElementModifier> ArmorElementModifiers = new List<ArmorElementModifier>();
    public Protection Protection;

    public DamageData Protect(DamageData damageData, DamageController damageController)
    {
      var result = RunProtection(damageData, damageController);
      result.DamageData.DamageAmount = CalculateDamage(result.DamageData);
      if (result.EffectProtection == EffectProtection.Block) result.DamageData.Effects = new DamageEffect[0];
      else damageData.Effects = damageData.Effects.Select(CalculateEffect).ToArray();
      return result.DamageData;
    }

    float CalculateDamage(DamageData damageData)
    {
      var armorElementModifier = ArmorElementModifiers.FirstOrDefault(x => x.Element == damageData.Element);
      float modifier = armorElementModifier?.Modifier ?? 1;
      float damageAmount = damageData.DamageAmount * modifier * DamageMultiplier;
      return damageAmount;
    }

    DamageEffect CalculateEffect(DamageEffect effect)
    {
      var armorElementModifier = ArmorElementModifiers.FirstOrDefault(x => x.Element == effect.Element);
      return armorElementModifier != null ? effect.Recalculate(armorElementModifier.Modifier) : effect;
    }

    ProtectionResult RunProtection(DamageData damageData, DamageController damageController)
    {
      return !Protection ? new ProtectionResult() { EffectProtection = EffectProtection.Ignore, DamageData = damageData } : Protection.RunProtection(damageData, damageController);
    }
  }
}
