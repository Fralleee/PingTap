using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fralle.Attack.Defense
{
  public class Armor : MonoBehaviour
  {
    public float DamageMultiplier => 1 - 0.06f * armor / (1 + 0.06f * armor);

    [SerializeField] int armor;
    [SerializeField] List<ArmorElementModifier> armorElementModifiers = new List<ArmorElementModifier>();
    [SerializeField] Protection protection;

    public Damage Protect(Damage damage, Health health)
    {
      ProtectionResult result = RunProtection(damage, health);
      result.damage.damageAmount = CalculateDamage(result.damage);
      if (result.effectProtection == EffectProtection.Block) result.damage.effects = new DamageEffect[0];
      else damage.effects = damage.effects.Select(CalculateEffect).ToArray();
      return result.damage;
    }

    float CalculateDamage(Damage damage)
    {
      ArmorElementModifier armorElementModifier = armorElementModifiers.FirstOrDefault(x => x.element == damage.element);
      float modifier = armorElementModifier?.modifier ?? 1;
      float damageAmount = damage.damageAmount * modifier * DamageMultiplier;
      return damageAmount;
    }

    DamageEffect CalculateEffect(DamageEffect effect)
    {
      ArmorElementModifier armorElementModifier = armorElementModifiers.FirstOrDefault(x => x.element == effect.element);
      return armorElementModifier != null ? effect.Recalculate(armorElementModifier.modifier) : effect;
    }

    ProtectionResult RunProtection(Damage damage, Health health)
    {
      if (!protection) return new ProtectionResult() { effectProtection = EffectProtection.Ignore, damage = damage };
      return protection.RunProtection(damage, health);
    }
  }
}
