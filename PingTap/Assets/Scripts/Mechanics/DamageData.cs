using System;
using UnityEngine;

namespace Fralle
{
  public class DamageData
  {
    public Player player;
    public Element element;
    public DamageEffect[] effects;
    public HitBoxType hitBoxType = HitBoxType.Major;
    public Vector3 position;
    public float hitAngle;
    public float damage;

    public DamageData()
    {
      effects = new DamageEffect[0];
    }

    public DamageData WithHitboxModifier()
    {
      switch (hitBoxType)
      {
        case HitBoxType.Major: return this;
        case HitBoxType.Nerve:
          damage *= 2.5f;
          return this;
        case HitBoxType.Minor:
          damage *= 0.75f;
          return this;
        default:
          return this;
      }
    }
  }
}