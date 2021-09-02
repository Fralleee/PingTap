using UnityEngine;

namespace Fralle.PingTap
{
  public enum HitArea
  {
    Leg,
    Chest,
    Pelvis,
    Head
  }

  public static class HitAreaMethods
  {
    public static float GetMultiplier(this HitArea ha)
    {
      return ha switch
      {
        HitArea.Leg => 0.75f,
        HitArea.Chest => 1.0f,
        HitArea.Pelvis => 1.25f,
        HitArea.Head => 4.0f,
        _ => 1.0f
      };
    }

    public static GameObject GetImpactEffect(this HitArea ha, DamageController damageController)
    {
      return ha switch
      {
        HitArea.Leg => damageController.impactEffect,
        HitArea.Chest => damageController.impactEffect,
        HitArea.Pelvis => damageController.impactEffect,
        HitArea.Head => damageController.impactEffect,
        _ => damageController.impactEffect
      };
    }
  }
}
