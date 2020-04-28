using UnityEngine;

namespace Fralle.Attack.Defense
{
  [CreateAssetMenu(menuName = "Attack/Protection/Back attack")]
  public class BackAttack : Protection
  {
    public override ProtectionResult RunProtection(Damage data, Health target)
    {
      if (!(data.hitAngle < 90) && !(data.hitAngle > 270))
        return new ProtectionResult() { effectProtection = EffectProtection.Ignore, damage = data };

      data.damageAmount = 0;
      data.hitBoxType = HitBoxType.Minor;
      return new ProtectionResult() { effectProtection = effectProtection, damage = data };
    }
  }
}