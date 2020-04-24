using Fralle;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack/Protection/Back attack")]
public class BackAttack : Protection
{
  public override ProtectionResult RunProtection(DamageData data, DamageController target)
  {
    if (!(data.hitAngle < 90) && !(data.hitAngle > 270)) return new ProtectionResult() { effectProtection = EffectProtection.BlockNone, damageData = data };

    data.damage = 0;
    data.hitBoxType = HitBoxType.Minor;
    return new ProtectionResult() { effectProtection = effectProtection, damageData = data };
  }
}