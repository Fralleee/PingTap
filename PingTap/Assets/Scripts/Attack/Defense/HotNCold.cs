using System.Linq;
using Fralle.Attack.Offense;
using UnityEngine;

namespace Fralle.Attack.Defense
{
  [CreateAssetMenu(menuName = "Attack/Protection/Hot and cold")]
  public class HotNCold : Protection
  {
    public override ProtectionResult RunProtection(Damage damage, Health target)
    {
      bool fireToWater = damage.element == Element.Fire && target.damageEffects.Any(x => x.element == Element.Water);
      if (fireToWater) return new ProtectionResult() { effectProtection = EffectProtection.Ignore, damage = damage };

      bool waterToFire = damage.element == Element.Water && target.damageEffects.Any(x => x.element == Element.Fire);
      if (waterToFire) return new ProtectionResult() { effectProtection = EffectProtection.Ignore, damage = damage };

      damage.damageAmount = 0;
      damage.hitBoxType = HitBoxType.Minor;
      return new ProtectionResult() { effectProtection = effectProtection, damage = damage };
    }
  }
}