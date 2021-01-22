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
    public float DamageMultiplier => 1 - 0.06f * armor / (1 + 0.06f * armor);

    public int armor;
    public List<ArmorElementModifier> armorElementModifiers = new List<ArmorElementModifier>();
    public Protection protection;

    public DamageData Protect(DamageData damageData, DamageController damageController)
    {
      var result = RunProtection(damageData, damageController);
      result.damageData.damageAmount = CalculateDamage(result.damageData);
      if (result.effectProtection == EffectProtection.Block) result.damageData.effects = new DamageEffect[0];
      else damageData.effects = damageData.effects.Select(CalculateEffect).ToArray();
      return result.damageData;
    }

    float CalculateDamage(DamageData damageData)
    {
      var armorElementModifier = armorElementModifiers.FirstOrDefault(x => x.element == damageData.element);
      var modifier = armorElementModifier?.modifier ?? 1;
      var damageAmount = damageData.damageAmount * modifier * DamageMultiplier;
      return damageAmount;
    }

    DamageEffect CalculateEffect(DamageEffect effect)
    {
      var armorElementModifier = armorElementModifiers.FirstOrDefault(x => x.element == effect.element);
      return armorElementModifier != null ? effect.Recalculate(armorElementModifier.modifier) : effect;
    }

    ProtectionResult RunProtection(DamageData damageData, DamageController damageController)
    {
      return !protection ? new ProtectionResult() { effectProtection = EffectProtection.Ignore, damageData = damageData } : protection.RunProtection(damageData, damageController);
    }
  }
}
