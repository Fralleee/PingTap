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
      var fireToWater = damageData.element == Element.Fire && damageController.damageEffects.Any(x => x.element == Element.Water);
      if (fireToWater) return new ProtectionResult() { effectProtection = EffectProtection.Ignore, damageData = damageData };

      var waterToFire = damageData.element == Element.Water && damageController.damageEffects.Any(x => x.element == Element.Fire);
      if (waterToFire) return new ProtectionResult() { effectProtection = EffectProtection.Ignore, damageData = damageData };

      damageData.damageAmount = 0;
      return new ProtectionResult() { effectProtection = effectProtection, damageData = damageData };
    }
  }
}