using CombatSystem.Combat.Damage;
using CombatSystem.Enums;
using System.Linq;
using UnityEngine;

namespace CombatSystem.Defense.Protections
{
  [CreateAssetMenu(menuName = "PlayerAttack/Protection/Hot and cold")]
  public class HotNCold : Protection
  {
    public override ProtectionResult RunProtection(DamageData damageData, DamageController damageController)
    {
      var fireToWater = damageData.Element == Element.Fire && damageController.DamageEffects.Any(x => x.Element == Element.Water);
      if (fireToWater) return new ProtectionResult() { EffectProtection = EffectProtection.Ignore, DamageData = damageData };

      var waterToFire = damageData.Element == Element.Water && damageController.DamageEffects.Any(x => x.Element == Element.Fire);
      if (waterToFire) return new ProtectionResult() { EffectProtection = EffectProtection.Ignore, DamageData = damageData };

      damageData.DamageAmount = 0;
      return new ProtectionResult() { EffectProtection = EffectProtection, DamageData = damageData };
    }
  }
}
