using Fralle.Attack.Offense;
using System.Linq;
using UnityEngine;

namespace Fralle.Attack.Defense
{
  [CreateAssetMenu(menuName = "PlayerAttack/Protection/Hot and cold")]
  public class HotNCold : Protection
  {
    public override ProtectionResult RunProtection(Damage damage, Health target)
    {
      var fireToWater = damage.element == Element.Fire && target.damageEffects.Any(x => x.element == Element.Water);
      if (fireToWater) return new ProtectionResult() { effectProtection = EffectProtection.Ignore, damage = damage };

      var waterToFire = damage.element == Element.Water && target.damageEffects.Any(x => x.element == Element.Fire);
      if (waterToFire) return new ProtectionResult() { effectProtection = EffectProtection.Ignore, damage = damage };

      damage.damageAmount = 0;
      return new ProtectionResult() { effectProtection = effectProtection, damage = damage };
    }
  }
}