using UnityEngine;

namespace Fralle.Attack
{
  public class Damage
  {
    public Player player;
    public Element element;
    public DamageEffect[] effects;
    public HitBoxType hitBoxType = HitBoxType.Major;
    public Vector3 position;
    public float hitAngle;
    public float damageAmount;

    public Damage()
    {
      effects = new DamageEffect[0];
    }

    public Damage WithHitboxModifier()
    {
      switch (hitBoxType)
      {
        case HitBoxType.Major: return this;
        case HitBoxType.Nerve:
          damageAmount *= 2.5f;
          return this;
        case HitBoxType.Minor:
          damageAmount *= 0.75f;
          return this;
        default:
          return this;
      }
    }
  }
}