using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fralle;
using UnityEngine;

public class Armor : MonoBehaviour
{
  public float DamageMultiplier => 1 - 0.06f * armor / (1 + 0.06f * armor);

  [SerializeField] int armor;
  [SerializeField] List<ArmorElementModifier> armorElementModifiers = new List<ArmorElementModifier>();
  [SerializeField] Protection protection;

  public ProtectionResult RunProtection(DamageData damageData, DamageController damageController)
  {
    if (!protection) return new ProtectionResult() {effectProtection = EffectProtection.BlockNone, damageData = damageData};
    return protection.RunProtection(damageData, damageController);
  }

  public float CalculateDamage(DamageData damageData, DamageController damageController)
  {
    ArmorElementModifier armorElementModifier = armorElementModifiers.FirstOrDefault(x => x.element == damageData.element);
    float modifier = armorElementModifier?.modifier ?? 1;
    float damage = damageData.damage * modifier * DamageMultiplier;
    return damage;
  }

  public DamageEffect CalculateEffect(DamageEffect effect, EffectProtection effectProtection)
  {
    if (effectProtection == EffectProtection.BlockDamage) effect.baseDamageModifier = 0;
    ArmorElementModifier armorElementModifier = armorElementModifiers.FirstOrDefault(x => x.element == effect.element);
    Debug.Log($"CalculateEffect gave damage {effect.baseDamageModifier}");
    return armorElementModifier != null ? effect.Recalculate(armorElementModifier.modifier) : effect;
  }

}