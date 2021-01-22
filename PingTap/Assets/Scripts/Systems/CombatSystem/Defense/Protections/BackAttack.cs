using CombatSystem.Combat.Damage;
using CombatSystem.Enums;
using UnityEngine;

namespace CombatSystem.Defense.Protections
{
  [CreateAssetMenu(menuName = "PlayerAttack/Protection/Back attack")]
  public class BackAttack : Protection
  {
    public override ProtectionResult RunProtection(DamageData damageData, DamageController damageController)
    {
      if (damageData.hitAngle < 0 || !(damageData.hitAngle < 90) && !(damageData.hitAngle > 270))
        return new ProtectionResult() { effectProtection = EffectProtection.Ignore, damageData = damageData };

      damageData.damageAmount = 0;
      return new ProtectionResult() { effectProtection = effectProtection, damageData = damageData };
    }
  }
}