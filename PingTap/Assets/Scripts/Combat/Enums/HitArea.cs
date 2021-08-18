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
      switch (ha)
      {
        case HitArea.Leg:
          return 0.75f;
        case HitArea.Chest:
          return 1.0f;
        case HitArea.Pelvis:
          return 1.25f;
        case HitArea.Head:
          return 4.0f;
        default:
          return 1.0f;
      }
    }

    public static GameObject GetImpactEffect(this HitArea ha, DamageController damageController)
    {
      switch (ha)
      {
        case HitArea.Leg:
          return damageController.impactEffect;
        case HitArea.Chest:
          return damageController.impactEffect;
        case HitArea.Pelvis:
          return damageController.impactEffect;
        case HitArea.Head:
          return damageController.impactEffect;
        default:
          return damageController.impactEffect;
      }
    }
  }
}
