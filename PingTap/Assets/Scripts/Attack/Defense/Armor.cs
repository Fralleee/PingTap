using Fralle.Attack.Effect;
using Fralle.Attack.Offense;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fralle.Attack.Defense
{
  [Serializable]
  public class Armor
  {
    public float DamageMultiplier => 1 - 0.06f * armor / (1 + 0.06f * armor);

    public int armor;
    public List<ArmorElementModifier> armorElementModifiers = new List<ArmorElementModifier>();
    public Protection protection;

    public Damage Protect(Damage damage, Health health)
    {
      var result = RunProtection(damage, health);
      result.damage.damageAmount = CalculateDamage(result.damage);
      if (result.effectProtection == EffectProtection.Block) result.damage.effects = new DamageEffect[0];
      else damage.effects = damage.effects.Select(CalculateEffect).ToArray();
      return result.damage;
    }

    float CalculateDamage(Damage damage)
    {
      var armorElementModifier = armorElementModifiers.FirstOrDefault(x => x.element == damage.element);
      var modifier = armorElementModifier?.modifier ?? 1;
      var damageAmount = damage.damageAmount * modifier * DamageMultiplier;
      return damageAmount;
    }

    DamageEffect CalculateEffect(DamageEffect effect)
    {
      var armorElementModifier = armorElementModifiers.FirstOrDefault(x => x.element == effect.element);
      return armorElementModifier != null ? effect.Recalculate(armorElementModifier.modifier) : effect;
    }

    ProtectionResult RunProtection(Damage damage, Health health)
    {
      return !protection ? new ProtectionResult() { effectProtection = EffectProtection.Ignore, damage = damage } : protection.RunProtection(damage, health);
    }
  }
}
