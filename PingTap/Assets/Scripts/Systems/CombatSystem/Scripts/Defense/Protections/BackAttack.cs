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
      if (damageData.HitAngle < 0 || !(damageData.HitAngle < 90) && !(damageData.HitAngle > 270))
        return new ProtectionResult() { EffectProtection = EffectProtection.Ignore, DamageData = damageData };

      damageData.DamageAmount = 0;
      return new ProtectionResult() { EffectProtection = EffectProtection, DamageData = damageData };
    }
  }
}