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
  [SerializeField] List<DamageProtection> damageProtections = new List<DamageProtection>();
  
  public float CalculateDamage(DamageData damageData)
  {
    foreach (DamageProtection protection in damageProtections)
    {
      damageData = protection.RunProtection(damageData, transform);
    }

    ArmorElementModifier armorElementModifier = armorElementModifiers.FirstOrDefault(x => x.element == damageData.element);
    float modifier = armorElementModifier?.modifier ?? 1;
    float damage = damageData.damage * modifier * DamageMultiplier;
    return damage;
  }

  public DamageEffect CalculateEffect(DamageEffect effect)
  {
    ArmorElementModifier armorElementModifier = armorElementModifiers.FirstOrDefault(x => x.element == effect.element);
    return armorElementModifier != null ? effect.Recalculate(armorElementModifier.modifier) : effect;
  }

}