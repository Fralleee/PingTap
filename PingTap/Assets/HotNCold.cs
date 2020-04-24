using System.Linq;
using Fralle;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack/Protection/Hot and cold")]
public class HotNCold : Protection
{
  public override ProtectionResult RunProtection(DamageData data, DamageController target)
  {
    bool fireToWater = data.element == Element.Fire && target.damageEffects.Any(x => x.element == Element.Water);
    if (fireToWater)
    {
      Debug.Log("Fire to water");
      return new ProtectionResult() { effectProtection = EffectProtection.BlockNone, damageData = data };
    }
    bool waterToFire = data.element == Element.Water && target.damageEffects.Any(x => x.element == Element.Fire);
    if (waterToFire)
    {
      Debug.Log("Water to fire");
      return new ProtectionResult() { effectProtection = EffectProtection.BlockNone, damageData = data };
    }

    data.damage = 0;
    data.hitBoxType = HitBoxType.Minor;
    return new ProtectionResult() { effectProtection = effectProtection, damageData = data };
  }
}